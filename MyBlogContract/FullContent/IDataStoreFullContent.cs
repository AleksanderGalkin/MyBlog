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
        IDataStoreRecord GetContent(int Id, int tempPostContentId=0);
        IEnumerable<IDataStoreRecord> GetAllContents();
        IEnumerable<IDataStoreRecord> GetDbPost(int PostId);
        IEnumerable<IDataStoreRecord> GetModPost(int PostId);
        void Clear();
    }

}
