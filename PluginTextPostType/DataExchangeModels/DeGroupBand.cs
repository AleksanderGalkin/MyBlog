
using MyBlogContract;
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    [Export("PluginTextPostType", typeof(IDeGroupBand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DeGroupBand : IDeGroupBand
    {
        public override int PostId { get; set; }
        public override int Order { get; set; }
        public override string AreaName { get; set; }

        public override string CallbackControllerName_CmdShowFullContent { get; set; }
        public override string CallbackActionName_CmdShowFullContent { get; set; }
        public override int ContentId_CmdShowFullContent { get; set; }

        public override string CallbackControllerName_CmdShowPostView { get; set; }
        public override string CallbackActionName_CmdShowPostView { get; set; }
        public override int PostId_CmdShowPostView { get; set; }

        public override RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("Order", this.Order.ToString());
            if (direction == DeDirection.ToPlugin)
            {
                result.Add("area", this.AreaName);
            }
            else
            {
                result.Add("area", "");
            }
            result.Add("AreaName", this.AreaName);
            result.Add("CallbackControllerName_CmdShowFullContent", this.CallbackControllerName_CmdShowFullContent);
            result.Add("CallbackActionName_CmdShowFullContent", this.CallbackActionName_CmdShowFullContent);
            result.Add("ContentId_CmdShowFullContent", this.ContentId_CmdShowFullContent.ToString());

            result.Add("CallbackControllerName_CmdShowPostView", this.CallbackControllerName_CmdShowPostView);
            result.Add("CallbackActionName_CmdShowPostView", this.CallbackActionName_CmdShowPostView);
            result.Add("PostId_CmdShowPostView", this.PostId_CmdShowPostView.ToString());

            return result;
        }

    }
}