using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.SessionSelect

{

    [InheritedExport]
    public interface IPluginSessionSelect<E>
        where E:  IDs

    {
        IList<E> all_entities { get; }
        IList<E> select_entities { get; set; }
        int getHash();
        void setHash(int HashCode);
        void setKeyName(String Key);
        string getKeyName();

    }
}
