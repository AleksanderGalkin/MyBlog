﻿using PluginImagePostType.DataExchangeModels;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace PluginImagePostType.Models
{
    public class VmDisplay: DataExchangeModelPostManage
    {

        public int tempPostContentId { get; set; }
        public string Comment { get; set; }
        [AllowHtml]
        public string Data { get; set; }
        public bool data_edit_diff_flag { get; set; }

       
        public int Order { get; set; }

        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = base.GetDictionary();

            result.Add("data_edit_diff_flag", data_edit_diff_flag);
            return result;
        }


    }
}