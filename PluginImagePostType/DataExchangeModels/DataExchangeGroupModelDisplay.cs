
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginImagePostType.DataExchangeModels
{
    [Export("PluginImagePostType", typeof(IDeGroupModelDisplay))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataExchangeGroupModelDisplay : IDeGroupModelDisplay
    {
        public override int PostId { get; set; }
        public override int Order { get; set; }
        public override string AreaName { get; set; }


        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("Order", this.Order.ToString());
            result.Add("AreaName", this.AreaName);
            result.Add("area", this.AreaName);

            return result;
        }

    }
}