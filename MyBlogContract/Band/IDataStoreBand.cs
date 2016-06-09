using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.Band
{
    [InheritedExport]
    public interface IDataStoreBand 
    {
        IDataStoreRecord Get(int Id);
        void Add(IDataStoreRecord Record);
        IEnumerable<IDataStoreRecord> Get();
        IEnumerable<IDataStoreRecord> GetPost(int PostId);
        void Clear();
    }

}
