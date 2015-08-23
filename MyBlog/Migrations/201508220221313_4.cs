namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "FullName");
            DropColumn("dbo.AspNetUsers", "IsNotificationAllowed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsNotificationAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
        }
    }
}
