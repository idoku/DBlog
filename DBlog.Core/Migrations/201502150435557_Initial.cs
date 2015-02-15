namespace DBlog.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PenName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Author = c.String(maxLength: 20),
                        CreateDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        HasCommentsEnabled = c.Boolean(nullable: false),
                        Slug = c.String(maxLength: 50),
                        IsTop = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Views = c.Int(nullable: false),
                        UpdatedBy_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Author", t => t.UpdatedBy_Id)
                .ForeignKey("dbo.Author", t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Descritption = c.String(maxLength: 500),
                        ParentCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.ParentCategory_Id)
                .Index(t => t.ParentCategory_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Author = c.String(maxLength: 30),
                        Email = c.String(),
                        Content = c.String(maxLength: 1000),
                        CommentDate = c.DateTime(nullable: false),
                        IsAudit = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        PostId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comment", t => t.ParentId)
                .ForeignKey("dbo.Post", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.BlogInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Board = c.String(maxLength: 200),
                        Email = c.String(maxLength: 200),
                        Copyright = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Author_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Author", t => t.Author_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        PostRefId = c.Int(nullable: false),
                        CategoryRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostRefId, t.CategoryRefId })
                .ForeignKey("dbo.Post", t => t.PostRefId, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryRefId, cascadeDelete: true)
                .Index(t => t.PostRefId)
                .Index(t => t.CategoryRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Author_Id", "dbo.Author");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Post", "CreatedBy_Id", "dbo.Author");
            DropForeignKey("dbo.Post", "UpdatedBy_Id", "dbo.Author");
            DropForeignKey("dbo.Comment", "PostId", "dbo.Post");
            DropForeignKey("dbo.Comment", "ParentId", "dbo.Comment");
            DropForeignKey("dbo.PostCategories", "CategoryRefId", "dbo.Category");
            DropForeignKey("dbo.PostCategories", "PostRefId", "dbo.Post");
            DropForeignKey("dbo.Category", "ParentCategory_Id", "dbo.Category");
            DropIndex("dbo.PostCategories", new[] { "CategoryRefId" });
            DropIndex("dbo.PostCategories", new[] { "PostRefId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Author_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Comment", new[] { "ParentId" });
            DropIndex("dbo.Comment", new[] { "PostId" });
            DropIndex("dbo.Category", new[] { "ParentCategory_Id" });
            DropIndex("dbo.Post", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Post", new[] { "UpdatedBy_Id" });
            DropTable("dbo.PostCategories");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.BlogInfo");
            DropTable("dbo.Comment");
            DropTable("dbo.Category");
            DropTable("dbo.Post");
            DropTable("dbo.Author");
        }
    }
}
