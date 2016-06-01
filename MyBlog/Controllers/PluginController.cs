using MyBlog.Infrastructure.Services;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlog.Controllers
{
    public class PluginController : Controller
    {
        // GET: Plugin
        public ActionResult RunPlugin(IDEModelPostManage Model)
        {
            string content_type_handler = "PluginTextPostType";
            //string version = "1.0";

            string controller_name = PlugInFactory.GetControllerNameByInterface(typeof(IBandDisplay), content_type_handler);
            string display_action_name = PlugInFactory.GetActionDisplayNameByInterface(typeof(IBandDisplay), content_type_handler);

            Model.Id = 1;

            RouteValueDictionary rvd = Model.GetDictionary();

            return RedirectToAction(display_action_name, controller_name, rvd);

        }
    }
}