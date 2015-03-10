namespace DBlog.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Comment", new[] { "ParentId" });
            AlterColumn("dbo.Comment", "CommentDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comment", "IsAudit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Comment", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Comment", "ParentId", c => c.Int());
            CreateIndex("dbo.Comment", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comment", new[] { "ParentId" });
            AlterColumn("dbo.Comment", "ParentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comment", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Comment", "IsAudit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Comment", "CommentDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Comment", "ParentId");
        }
    }
}
