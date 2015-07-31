namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Sex", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Sex", c => c.String(nullable: false, maxLength: 6));
        }
    }
}
