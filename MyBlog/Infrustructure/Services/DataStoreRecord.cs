using MyBlogContract;
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure.Sevices
{
    [Export(typeof(IDataStoreRecord))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataStoreRecord: IDataStoreRecord
    {
        override public int PostId { get; set; }
        override public int PostContentId { get; set; }
        override public int PostContentIdForNewRecords { get; set; }
        override public int LikePlus { get; set; }
        override public int LikeMinus { get; set; }
        override public byte[] ContentData { get; set; }
        override public string Comment { get; set; }

        override public string ContentPluginName { get; set; }
        override public string ContentPluginVersion { get; set; }

        override public bool IsInGroup { get; set; }
        override public int Order { get; set; }

        override public IDataStoreRecordStatus Status { get; set; }
    }
}