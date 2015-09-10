namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PostComment", "PostId");
            CreateIndex("dbo.PostTag", "PostId");
            CreateIndex("dbo.PostView", "PostId");
            AddForeignKey("dbo.PostComment", "PostId", "dbo.Post", "PostId", cascadeDelete: true);
            AddForeignKey("dbo.PostTag", "PostId", "dbo.Post", "PostId", cascadeDelete: true);
            AddForeignKey("dbo.PostView", "PostId", "dbo.Post", "PostId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostView", "PostId", "dbo.Post");
            DropForeignKey("dbo.PostTag", "PostId", "dbo.Post");
            DropForeignKey("dbo.PostComment", "PostId", "dbo.Post");
            DropIndex("dbo.PostView", new[] { "PostId" });
            DropIndex("dbo.PostTag", new[] { "PostId" });
            DropIndex("dbo.PostComment", new[] { "PostId" });
        }
    }
}
