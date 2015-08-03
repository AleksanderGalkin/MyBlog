using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure
{
    public class UnitOfWork:IUnitOfWork
    {
        public IDbContext db { get; private set; }
        public UnitOfWork (IDbContext DbContext)
        {
            db = DbContext;
        }
        public void BeginTransaction()
        {
            System.Diagnostics.Debug.WriteLine("BeginTransaction");
            db = new ApplicationDbContext();
        }
        public void Commit()
        {
            System.Diagnostics.Debug.WriteLine("Commit");
            db.SaveChanges();
        }
    }
}