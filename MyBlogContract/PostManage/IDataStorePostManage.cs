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
        void Add(IDataStoreRecord Record);
        IEnumerable<IDataStoreRecord> Get();
        IDataStoreRecord Get(int Id, int _temporary_PostContentId = 0);
        IDataStoreRecord GetNew();
        void Modify(IDataStoreRecord Model);
        void Delete (int Id, int _temporary_PostContentId = 0);
        void Create(IDataStoreRecord Model);
        void Clear();
    }
}
