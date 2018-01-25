using MyBlogContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTag.SelectListData
{
    public class DtoDisplayIn: IDto
    {
        public string StoreModelKey { get; set; }

        public string on_change_event { get; set; }


        public string CmdGetResult_CallbackControllerName { get; set; }
        public string CmdGetResult_CallbackActionName { get; set; }
        public string CmdGetResult_AreaName { get; set; }
        public string CmdGetResult_ResultLocationId { get; set; }



    }
}