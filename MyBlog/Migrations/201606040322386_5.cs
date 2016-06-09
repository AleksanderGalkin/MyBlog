namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "PostPluginName", c => c.String());
            AddColumn("dbo.PostContent", "ContentPluginName", c => c.String());
            AddColumn("dbo.PostContent", "IsInGroup", c => c.Boolean(nullable: false));
            AddColumn("dbo.PostContent", "Order", c => c.Int(nullable: false));
            Sql("update dbo.PostContent set ContentPluginName = ContentType");
            Sql("update dbo.Post set PostPluginName = (select top 1 ContentPluginName from dbo.PostContent where dbo.Post.PostId = dbo.PostContent.PostId )");
            DropColumn("dbo.PostContent", "ContentDataType");
            DropColumn("dbo.PostContent", "ContentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PostContent", "ContentType", c => c.String());
            AddColumn("dbo.PostContent", "ContentDataType", c => c.Int(nullable: false));
            DropColumn("dbo.PostContent", "Order");
            DropColumn("dbo.PostContent", "IsInGroup");
            DropColumn("dbo.PostContent", "ContentPluginName");
            DropColumn("dbo.Post", "PostPluginName");
        }
    }
}
