
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.TagManage;
using PluginTag.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTag.DataExchangeModels
{
    [Export( typeof(IDtoTag))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DtoTag : IDtoTag
    {
        public override int PostId { get; set; }
        public override int TagId { get; set; }
        public override string AreaName { get { return AppSettings.PluginName; } }
        public override string StoreModelKey { get; set; }


        public override string CallbackControllerName_CmdShowPostByTag { get; set; }
        public override string CallbackActionName_CmdShowPostByTag { get; set; }
        public override int TagId_CmdShowPostByTag { get; set; }

        public override string CmdShowParentPost_CallbackControllerName { get; set; }
        public override string CmdShowParentPost_CallbackActionName { get; set; }
        public override int CmdShowParentPost_PostId { get; set; }


        public override RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
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

            result.Add("CmdShowParentPost_CallbackControllerName", this.CmdShowParentPost_CallbackControllerName);
            result.Add("CmdShowParentPost_CallbackActionName", this.CmdShowParentPost_CallbackActionName);
            result.Add("CmdShowParentPost_PostId", this.CmdShowParentPost_PostId.ToString());

            return result;
        }

    }
}