
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using PluginTextPostType.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginTextPostType.Infrastucture
{
    [Export(typeof(IPlugIn))]
    public  class Plugin1Entry :  IPlugIn
    {
        public  string PluginName
        {
            get
            {
                return AppSettings.PluginName;
            }
        }
        public string Version
        {
            get
            {
                return AppSettings.Version;
            }
        }

    }

    public class Plugin1AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return AppSettings.PluginName; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AppSettings.PluginName+"_default",
                AppSettings.PluginName+"/{controller}/{action}/{id}",
                new { controller = "Band", action = "Display", id = UrlParameter.Optional },
                namespaces: new[] { AppSettings.PluginName + ".Controllers" }
                );

            AutoMapperConfig.RegisterMappings();
        }
    }
}