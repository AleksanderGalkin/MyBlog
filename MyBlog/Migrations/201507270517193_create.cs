namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Mail = c.String(nullable: false),
                        IsMailSended = c.Boolean(nullable: false),
                        IsDeliveryError = c.Boolean(nullable: false),
                        IsUserBack = c.Boolean(nullable: false),
                        IsUserChangePassword = c.Boolean(nullable: false),
                        IsUserConfirmRegistration = c.Boolean(nullable: false),
                        RowVer = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsNotificarionAllowed = c.Boolean(nullable: false),
                        Sex = c.String(nullable: false, maxLength: 6),
                        RowVer = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Registrations", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Id", "dbo.Registrations");
            DropIndex("dbo.Users", new[] { "Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Registrations");
        }
    }
}
