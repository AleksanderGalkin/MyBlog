namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostContent", "ContentData", c => c.Binary());
            AddColumn("dbo.PostContent", "ContentDataType", c => c.Int(nullable: false));
            AddColumn("dbo.PostContent", "Comment", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostContent", "Comment");
            DropColumn("dbo.PostContent", "ContentDataType");
            DropColumn("dbo.PostContent", "ContentData");
        }
    }
}
