using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTextPostType.Models
{
    public class VmItemGroup
    {
        public int Order { get; set; }
        public IEnumerable<VmItem> VmItems  { get;set;}
    }
}