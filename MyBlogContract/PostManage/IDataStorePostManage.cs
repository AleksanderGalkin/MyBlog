using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.PostManage
{
    [InheritedExport]
    public interface IDataStorePostManage
    {
         IEnumerable<IDataStoreRecord> GetAllContents();
         void Clear();

        IDataStoreRecord GetContent(int Id, int tempPostContentId=0);
        IEnumerable<IDataStoreRecord> GetDbPost(int PostId);
        IEnumerable<IDataStoreRecord> GetModPost(int PostId);
        IDataStoreRecord GetNew();
        void Modify(IDataStoreRecord Model);
        void Delete(int PostContentId, int tempPostContentId = 0);
       
    }
}
