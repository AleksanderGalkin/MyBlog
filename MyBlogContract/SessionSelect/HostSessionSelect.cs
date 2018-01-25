using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.SessionSelect
{
    public interface IHostSessionSelect< E>
        where E :  class, IDs
    {

        IPluginSessionSelect<E> getPluginSessionSelect();
        void update_from(IPluginSessionSelect<E> PlaginSessionSelects);
        IList<E> get_select_entities();


    }
}
