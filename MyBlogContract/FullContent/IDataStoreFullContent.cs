using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.FullContent
{
    [InheritedExport]
    public interface IDataStoreFullContent
    {
        IDataStoreRecord Get(int Id);
        void Add(IDataStoreRecord Record);
        IEnumerable<IDataStoreRecord> Get();
        IEnumerable<IDataStoreRecord> GetPost(int PostId);
        void Clear();
    }

}
