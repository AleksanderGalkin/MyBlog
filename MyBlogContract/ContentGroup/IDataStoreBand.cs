using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.ContentGroup
{
    [InheritedExport]
    public interface IDataStoreContentGroup
    {
        IDataStoreRecord GetContent(int Id, int tempPostContentId = 0);
        IEnumerable<IDataStoreRecord> GetAllContents();
        IEnumerable<IDataStoreRecord> GetDbPost(int PostId);
        IEnumerable<IDataStoreRecord> GetModPost(int PostId);
        IEnumerable<IDataStoreRecord> GetActualPost(int PostId);
        IEnumerable<IDataStoreRecord> GetGroupContent(int PostId, int Order);
        void Clear();
    }

}
