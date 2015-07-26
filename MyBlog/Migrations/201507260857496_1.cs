namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "Id", newName: "RegistrationId");
            RenameIndex(table: "dbo.Users", name: "IX_Id", newName: "IX_RegistrationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_RegistrationId", newName: "IX_Id");
            RenameColumn(table: "dbo.Users", name: "RegistrationId", newName: "Id");
        }
    }
}
