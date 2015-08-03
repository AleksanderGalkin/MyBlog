using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure
{
    public interface IUnitOfWork
    {
        IDbContext db { get; }
        void BeginTransaction();
        void Commit();

    }
}