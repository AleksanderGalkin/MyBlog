
using MyBlogContract.PostManage;
using PluginTextPostType.Infrastructure;
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
    public class DeModelPostManage :  IDEModelPostManage
    {

        public override int PostId { get; set; }
        public override int PostContentId { get; set; }
        public override int tempPostContentId { get; set; }

        public override string AreaName { get { return AppSettings.PluginName; } }



        public override string CallbackControllerName { get; set; }
        public override string CallbackActionName { get; set; }
        public override string List_content_insert_before_Id { get; set; }
        public override string Update_area_replace_Id { get; set; }
        public override string OnSuccessRemoveCallback { get; set; }
        public override bool data_edit_diff_flag { get; set; }

        override public RouteValueDictionary GetDictionary(MyBlogContract.DeDirection direction = MyBlogContract.DeDirection.ToPlugin)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("PostId", this.PostId.ToString());
            result.Add("PostContentId", this.PostContentId.ToString());
            result.Add("tempPostContentId", this.tempPostContentId.ToString());
            if (direction == MyBlogContract.DeDirection.ToPlugin)
            {
                result.Add("area", this.AreaName);
            }
            result.Add("AreaName", this.AreaName);

            result.Add("CallbackControllerName", this.CallbackControllerName);
            result.Add("CallbackActionName", this.CallbackActionName);
            result.Add("List_content_insert_before_Id", this.List_content_insert_before_Id);
            result.Add("Update_area_replace_Id", this.Update_area_replace_Id);
            result.Add("OnSuccessRemoveCallback", this.OnSuccessRemoveCallback);
            result.Add("data_edit_diff_flag", this.data_edit_diff_flag);
            return result;
        }

    }
}