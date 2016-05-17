using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract
{
    public enum IDataStoreRecordStatus {None, Created, Modified, Deleted};
    public abstract class IDataStoreRecord
    {

        virtual public int PostId { get; set; }
        virtual public int PostContentId { get; set; }
        virtual public int _temporary_PostContentId { get; set; }
        virtual public int LikePlus { get; set; }
        virtual public int LikeMinus { get; set; }
        virtual public byte[] ContentData { get; set; }
        virtual public string Comment { get; set; }
        virtual public string DataPluginName { get; set; }
        virtual public string DataPluginVersion { get; set; }
        virtual public IDataStoreRecordStatus Status { get; set; }

    }
}
