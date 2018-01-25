
using MyBlogContract.Band;
using MyBlogContract.ContentGroup;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using MyBlogContract;
using PluginTextPostType.Infrastructure;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType", typeof(IDeContentGroup))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DeContentGroup : IDeContentGroup
    {
        public override int PostId { get; set; }
        public override int Order { get; set; }
        public override string AreaName { get { return AppSettings.PluginName; } }


        public override RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("Order", this.Order.ToString());
            result.Add("AreaName", this.AreaName);
            if (direction == DeDirection.ToPlugin)
            {
                result.Add("area", this.AreaName);
            }
            else
            {
                result.Add("area", "");
            }

            return result;
        }

       
    }
}