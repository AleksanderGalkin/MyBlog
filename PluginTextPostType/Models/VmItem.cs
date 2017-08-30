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
    public class VmItem: DeItem
    {

        public string Comment { get; set; }
        [AllowHtml]
        public string Data { get; set; }


        public override RouteValueDictionary GetDictionary(MyBlogContract.DeDirection direction = MyBlogContract.DeDirection.ToPlugin)
        {
            RouteValueDictionary result = base.GetDictionary(direction);

            //result.Add("data_edit_diff_flag", data_edit_diff_flag);
            return result;
        }


    }
}