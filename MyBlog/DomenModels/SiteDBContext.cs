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
            : base("SiteDBConnection_")
        {}
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}