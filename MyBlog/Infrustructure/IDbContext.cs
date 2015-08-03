using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure
{
    public interface IDbContext:IDisposable
    {
        IDbSet<ApplicationUser> Users { get; set; }
      //  IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
    }
}