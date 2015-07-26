using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyBlog.DomenModels
{
    public class SiteDBContext:DbContext
    {
        public SiteDBContext()
            : base("SiteDBConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SiteDBContext, MyBlog.Migrations.Configuration>("SiteDBConnection"));
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}