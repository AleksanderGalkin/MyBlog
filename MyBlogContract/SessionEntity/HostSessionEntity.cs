using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.SessionEntity
{
    public interface IHostSessionEntity< E>
        //where T : IDbSet<E>
        where E :  class, IDs
    {
        IPluginSessionEntity<E> getPluginSessionEntity();
        void update_from(IPluginSessionEntity<E> PlaginSessionEntities);
        void update_to<T>(IDbSet<T> DbSetEntities) where T: class;
        IList<E> getEntitySet();

    }
}
