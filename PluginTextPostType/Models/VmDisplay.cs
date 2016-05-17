using MyBlogContract.PostManage;
using PluginTextPostType.DataExchangeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PluginTextPostType.Models
{
    public class VmDisplay: DataExchangeModelPostManage
    {
        // public int Id { get; set; }
        // public int PostId { get; set; }
        public int _temporary_PostContentId { get; set; }
        public string Comment { get; set; }
        [AllowHtml]
        public string Data { get; set; }
        public bool data_edit_diff_flag { get; set; }

        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = base.GetDictionary();
            result.Add("PostId",PostId);
            result.Add("data_edit_diff_flag", data_edit_diff_flag);
            return result;
        }

        // public string UpdateTargetId { get; set; }
        // public string OnSuccessRemoveCallback { get; set; }
    }
}