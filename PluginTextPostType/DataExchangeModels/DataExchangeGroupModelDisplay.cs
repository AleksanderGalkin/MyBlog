
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType", typeof(IDeGroupModelDisplay))]
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
            result.Add("area", this.AreaName);
            result.Add("AreaName", this.AreaName);

            return result;
        }

    }
}