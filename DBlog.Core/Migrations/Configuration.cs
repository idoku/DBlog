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

            //init category
            var rootCategory = new Category() { 
                 Id=1, Name = "Work", Descritption="my workd"
            };
            var childrenCategory = new Category() { 
                 Id=2, Name=".Net", Descritption=".net tech", ParentCategory=rootCategory
            };
            context.Categories.AddOrUpdate(x => x.Id, rootCategory, childrenCategory);
            
            //init post
            var post = new Post()
            {
                Id = 1,
                Author = author.PenName,
                CreatedBy = author,
                Categories = new List<Category>() { 
                 rootCategory,
                 childrenCategory,
                },
                CreateDate = DateTime.Now,
                Title = "my first .net article",
                Content = "first .net article,about .net",
                Description = ".net article",
                HasCommentsEnabled = false,
                IsDelete = false,
                IsTop = true,                
            };
            context.Posts.AddOrUpdate(x => x.Id, post);

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
