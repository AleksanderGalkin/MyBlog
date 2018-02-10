
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.TagCloud;
using MyBlogContract.TagManage;
using PluginTagCloud.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTagCloud.Dto
{
    [Export( typeof(IDtoTagCloud))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DtoTagCloud : IDtoTagCloud
    {
        public override int TagId { get; set; }
        public override string AreaName { get { return AppSettings.PluginName; } }
        public override string StoreModelKey { get; set; }


        public override string CallbackControllerName_CmdShowPostByTag { get; set; }
        public override string CallbackActionName_CmdShowPostByTag { get; set; }
        public override int TagId_CmdShowPostByTag { get; set; }


        public override RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("TagId", this.TagId.ToString());
            if (direction == DeDirection.ToPlugin)
            {
                result.Add("Area", this.AreaName);
            }
            else
            {
                result.Add("Area", "");
            }
            result.Add("AreaName", this.AreaName);
            result.Add("StoreModelKey", this.StoreModelKey);
            result.Add("CallbackControllerName_CmdShowPostByTag", this.CallbackControllerName_CmdShowPostByTag);
            result.Add("CallbackActionName_CmdShowPostByTag", this.CallbackActionName_CmdShowPostByTag);
            result.Add("TagId_CmdShowPostByTag", this.TagId_CmdShowPostByTag.ToString());

            return result;
        }

    }
}