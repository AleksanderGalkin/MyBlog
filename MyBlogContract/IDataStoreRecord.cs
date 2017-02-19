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

        abstract public int PostId { get; set; }
        abstract public int PostContentId { get; set; }
        abstract public int tempPostContentId { get; set; }
        abstract public int LikePlus { get; set; }
        abstract public int LikeMinus { get; set; }
        abstract public byte[] ContentData { get; set; }
        abstract public string Comment { get; set; }

        abstract public string ContentPluginName { get; set; }
        abstract public string ContentPluginVersion { get; set; }

        abstract public bool IsInGroup { get; set; }
        abstract public int Order { get; set; }

        abstract public IDataStoreRecordStatus Status { get; set; }

    }

}
