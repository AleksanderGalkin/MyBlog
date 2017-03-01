namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PostTag", "TagId");
            AddForeignKey("dbo.PostTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostTag", "TagId", "dbo.Tag");
            DropIndex("dbo.PostTag", new[] { "TagId" });
        }
    }
}
