
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType", typeof(IDEModelFullContent))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataExchangeFullContent : IDEModelFullContent
    {
        
        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("Id", this.Id.ToString());
            result.Add("PostId", this.PostId.ToString());

            return result;
        }

    }
}