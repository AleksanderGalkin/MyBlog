
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
        
        public override RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("Id", this.Id.ToString());

            return result;
        }

    }
}