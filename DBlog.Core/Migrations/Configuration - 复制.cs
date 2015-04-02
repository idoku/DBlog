namespace DBlog.Core.Migrations
{
    using DBlog.Core.Database;
    using DBlog.Core.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DBlog.Core.Database.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DBlog.Core.Database.BlogContext context)
        {
            //var userManager = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(new BlogContext()));
            //var roleManager = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(new BlogContext()));

            //string name = "doku";
            //string email = "dragon.ice@foxmail.com";
            //string password = "123@com";
            //string roleName = "Admin";
            ////init user
            //var user = new ApplicationUser()
            //{
            //    UserName = name,
            //    Email = email,
            //};
            //userManager.Create(user, password);
            ////init role
            //var role = new IdentityRole(roleName);
            //roleManager.Create(role);
            ////init user in role
            //userManager.AddToRole(user.Id, role.Name);

            //init blogInfo
            context.BlogInfos.AddOrUpdate(x => x.Id,
                new BlogInfo() { Id = 1, Email = "dragon.ice.995@hotmail.com", Copyright = "Copyright © 2015 , doku", Board = "hello world." });
            var author = new Author() { Id = 1, PenName = "Doku" };
            //init author
            context.Authors.AddOrUpdate(x => x.Id,
              author);


        }
    }
}
