﻿
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType", typeof(IDEModelDisplay))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataExchangeModelDisplay : IDEModelDisplay
    {
        public override int PostId { get; set; }
        public override int PostContentId { get; set; }
        public override string area { get; set; }


        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("PostContentId", this.PostContentId.ToString());
            result.Add("area", this.area);

            return result;
        }

    }
}