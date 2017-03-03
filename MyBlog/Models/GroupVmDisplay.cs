using MyBlogContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class GroupVmDisplay
    {
        public int PostId { get; set; }
        public int Order { get; set; }
        public bool IsInGroup { get; set; }
        public string GroupPluginName { get; set; }
        public IEnumerable<IDataStoreRecord> Contexts  { get;set;}
    }
}