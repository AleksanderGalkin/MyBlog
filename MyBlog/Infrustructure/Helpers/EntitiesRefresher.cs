using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace MyBlog.Infrustructure.Helpers
{
    public static  class EntitiesRefresher
    {
        public static IDbContext RefreshEntites(this IDbContext dbContext, RefreshMode refreshMode, Type entityType)
        {
            //https://christianarg.wordpress.com/2013/06/13/entityframework-refreshall-loaded-entities-from-database/
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var refreshableObjects = objectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
                .Where(x => entityType == null || x.Entity.GetType() == entityType)
                .Where(entry => entry.EntityKey != null)
                .Select(e => e.Entity)
                .ToArray();

            objectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);

            return dbContext;
        }

        public static IDbContext RefreshAllEntites(this IDbContext dbContext, RefreshMode refreshMode)
        {
            return RefreshEntites(dbContext: dbContext, refreshMode: refreshMode, entityType: null); //null entityType is a wild card
        }

        public static IDbContext RefreshEntites<TEntity>(this IDbContext dbContext, RefreshMode refreshMode)
        {
            return RefreshEntites(dbContext: dbContext, refreshMode: refreshMode, entityType: typeof(TEntity));
        }
    }
}

