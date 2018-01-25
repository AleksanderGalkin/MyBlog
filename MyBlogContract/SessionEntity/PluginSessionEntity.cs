using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.SessionEntity

{
    public enum IDataStoreRecordStatus2 { None, Created, Modified, Deleted };

    [InheritedExport]
    public interface IPluginSessionEntity<E>
        //where T: IDbSet<E> 
        where E:  IDs

    {
        void create(E Entity);
        void modify(E EntityNew);
        void delete(E Entity);
        IDictionary<E, IDataStoreRecordStatus2> data();
        IList<E> entities();
        IList<E> current_entities { get; set; }
        int getHash();
        void setHash(int HashCode);
        void setKeyName(String Key);
        string getKeyName();
        E newObj();

    }
}
