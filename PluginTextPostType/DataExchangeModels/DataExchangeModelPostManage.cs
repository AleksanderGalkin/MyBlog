
using MyBlogContract.PostManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType",typeof(IDEModelPostManage))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataExchangeModelPostManage :  IDEModelPostManage
    {




        override public RouteValueDictionary GetDictionary()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("Id", this.Id.ToString());
            result.Add("CallbackControllerName", this.CallbackControllerName);
            result.Add("CallbackActionName", this.CallbackActionName);
            result.Add("UpdateTargetId", this.UpdateTargetId);
            result.Add("OnSuccessRemoveCallback", this.OnSuccessRemoveCallback);
            return result;
        }

    }
}