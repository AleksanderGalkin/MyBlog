using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTextPostType.Models
{
    public class GroupVmDisplay
    {
        public int Order { get; set; }
        public IEnumerable<VmDisplay> VmDisplays  { get;set;}
    }
}