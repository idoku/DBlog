namespace DBlog.Core.Migrations
{
    using DBlog.Core.Database;
    using DBlog.Core.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DBlog.Core.Database.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DBlog.Core.Database.BlogContext context)
        {

            //init blogInfo
            context.BlogInfos.AddOrUpdate(x => x.Id,
                new BlogInfo() { Id = 1, Email = "dragon.ice.995@hotmail.com", Copyright = "Copyright © 2015 , doku", Board = "hello world." });
            var author = new Author() { Id = 1, PenName = "Doku" };
            //init author
            context.Authors.AddOrUpdate(x => x.Id,
              author);

            //init tags
            for (int i = 0; i < 20; i++)
            {
                var tag = new Tag() { 
                 TagName = "tag"+i.ToString(),
                  TagCount = 0                
                };
                context.Tags.AddOrUpdate(x => x.Id, tag);
            }

            //init category
            for (int i = 0; i < 10; i++)
            {
            
                var rootCategory = new Category()
                {
                    Id = i,
                    Name = "Work" + i,
                    Descritption = "my workd"
                };
                var childrenCategory = new Category()
                {
                    Id = i * 10,
                    Name = ".Net" + i,
                    Descritption = ".net tech",
                    ParentCategory = rootCategory
                };
                context.Categories.AddOrUpdate(x => x.Id, rootCategory, childrenCategory);
            }
          

            for (int i = 0; i < 15; i++)
            {
                var rootCategory = context.Categories.Find(1);
                var childrenCategory = context.Categories.Find(11);
                //init post
                var post = new Post()
                {
                    Id = i,
                    Author = author.PenName,
                    CreatedBy = author,
                    Slug = "myfirstnetarticle" + i.ToString(),
                    Categories = new List<Category>() { 
                 rootCategory,
                 childrenCategory,
                },
                    CreateDate = DateTime.Now,
                    Title = "my first .net article" + i.ToString(),
                    Content = "first .net article,about .net",
                    Description = ".net article",
                    HasCommentsEnabled = false,
                    IsDelete = false,
                    IsTop = true,
                };
                context.Posts.AddOrUpdate(x => x.Id, post);
            }

            var posted = context.Posts.Find(1);
            //init comment
            if (posted != null)
            {
                var comment = new Comment()
                {
                    CommentDate = DateTime.Now,
                    Author = "doku",
                    PostId = posted.Id,
                    IsAudit = false,
                    IsDeleted = false,
                    Email = "doku@doku.com",
                    Content = "abc",
                };
                context.Comments.AddOrUpdate(comment);
            }


            //init account
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new BlogContext()));
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new BlogContext()));

            string name = "doku";
            string email = "dragon.ice@foxmail.com";
            string password = "123@com";
            string roleName = "Admin";
            //init user
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = name,
                    Email = email,
                    Author = author
                };
                userManager.Create(user, password);
            }
            //init role
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
            //init user in role
            var roleForUser = userManager.GetRoles(user.Id);
            if (!roleForUser.Contains(role.Name))
            {
                userManager.AddToRole(user.Id, role.Name);
            }         
        }
    }
}
