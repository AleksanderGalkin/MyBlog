using PluginImagePostType.DataExchangeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginImagePostType.Models
{
    public class GroupVmDisplay: DataExchangeModelPostManage
    {
        
        public int Order { get; set; }
        
        public bool data_edit_diff_flag { get; set; }
        public IEnumerable<VmDisplay> VmDisplays  { get;set;}

        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = base.GetDictionary();

            result.Add("data_edit_diff_flag", data_edit_diff_flag);
            return result;
        }

    }
}