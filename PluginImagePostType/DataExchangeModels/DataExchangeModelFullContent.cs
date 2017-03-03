
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginImagePostType.DataExchangeModels
{
    [Export("PluginImagePostType", typeof(IDEModelFullContent))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataExchangeFullContent : IDEModelFullContent
    {
        public override int PostId { get; set; }
        public override int PostContentId { get; set; }
        public override string AreaName { get; set; }

        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("PostContentId", this.PostContentId.ToString());
            result.Add("AreaName", this.AreaName);
            result.Add("area", this.AreaName);

            return result;
        }

    }
}