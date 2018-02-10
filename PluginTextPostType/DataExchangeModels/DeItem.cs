
using MyBlogContract;
using MyBlogContract.Band;
using PluginTextPostType.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTextPostType.DataExchangeModels
{
    //[Export("PluginTextPostType", typeof(IDeItemBand))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class DeItem : IDeItemBand
    {
        public override int PostId { get; set; }
        public override int PostContentId { get; set; }
        public override string AreaName { get { return AppSettings.PluginName; } }

        public override int tempPostContentId { get; set; }


        public override string CallbackControllerName_CmdShowFullContent { get; set; }
        public override string CallbackActionName_CmdShowFullContent { get; set; }
        public override string CallbackControllerName_CmdShowPostView { get; set; }
        public override string CallbackActionName_CmdShowPostView { get; set; }
        public override int PostId_CmdShowPostView { get; set; }

        public override string CallbackControllerName_CmdShowBand { get; set; }

        public override string CallbackActionName_CmdShowBand { get; set; }



        public override RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("PostContentId", this.PostContentId.ToString());
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

            result.Add("CallbackControllerName_CmdShowPostView", this.CallbackControllerName_CmdShowPostView);
            result.Add("CallbackActionName_CmdShowPostView", this.CallbackActionName_CmdShowPostView);
            result.Add("PostId_CmdShowPostView", this.PostId_CmdShowPostView.ToString());

            result.Add("CallbackControllerName_CmdShowBand", this.CallbackControllerName_CmdShowBand);
            result.Add("CallbackActionName_CmdShowBand", this.CallbackActionName_CmdShowBand);

            return result;
        }

    }
}