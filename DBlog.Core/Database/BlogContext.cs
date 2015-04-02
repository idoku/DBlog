using DBlog.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBlog.Core.Database
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(BlogContext));

        public BlogContext()
            : base("BlogContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public IDbSet<Post> Posts { get; set; }

        public IDbSet<BlogInfo> BlogInfos { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Author>  Authors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Post>()
                .HasMany<Category>(p => p.Categories)
                .WithMany(c => c.Posts)
                .Map(cp => {
                    cp.MapLeftKey("PostRefId");
                    cp.MapRightKey("CategoryRefId");
                    cp.ToTable("PostCategories");
                });

            modelBuilder.Entity<Post>()
                .HasMany<Tag>(p => p.Tags)
                .WithMany(t => t.Posts)
                .Map(tp => {
                    tp.MapLeftKey("PostRefId");
                    tp.MapRightKey("TagRefId");
                    tp.ToTable("PostTags");
                });
        }



        public static BlogContext Create()
        {
            return new BlogContext();
        }

        //public override async Task<int> SaveChangesAsync()
        //{
        //    // Set modification date if an entity has been modified
        //    foreach (var entity in this.ChangeTracker.Entries<EntityBase>().Where(e => e.State == System.Data.Entity.EntityState.Modified))
        //    {
        //        entity.Entity.Modified = DateTime.Now;
        //    }

        //    try
        //    {
        //        return await base.SaveChangesAsync();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var errors = ex.EntityValidationErrors
        //                .Where(e => !e.IsValid)
        //                .Select(e => e.Entry.Entity.GetType().Name + " - Errors: " + string.Join(", ", e.ValidationErrors.Select(v => v.PropertyName + ": " + v.ErrorMessage)));

        //        string errorText = string.Join("\r\n", errors);
        //        Logger.Error(string.Format("Saving to database failed (Errors: {0})", errorText), ex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("Saving to database failed", ex);
        //        throw;
        //    }
        //}
       
    }
}
