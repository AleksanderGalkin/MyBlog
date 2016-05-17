using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrastructure.Services
{
    public class CustomViewEngine : RazorViewEngine
    {
        private List<string> _plugins = new List<string>();

        public CustomViewEngine(List<string> plugin_names)
        {
            _plugins = plugin_names;

            ViewLocationFormats = GetViewLocations();
            MasterLocationFormats = GetMasterLocations();
            PartialViewLocationFormats = GetViewLocations();
        }

        public string[] GetViewLocations()
        {
            var views = new List<string>();

            _plugins.ForEach(plugin =>
                views.Add("~/"+ AppSettings.PluginDirectory + "/" + plugin + "/Views/{1}/{0}.cshtml")
            );

            var base_views = base.ViewLocationFormats;
            
            views.AddRange(base_views);
            return views.ToArray();
        }

        public string[] GetMasterLocations()
        {
            var masterPages = new List<string>();


            _plugins.ForEach(plugin =>
                masterPages.Add("~/" + AppSettings.PluginDirectory + "/" + plugin + "/Views/Shared/{0}.cshtml")
            );
            var base_masterPages = base.MasterLocationFormats;
            masterPages.AddRange(base_masterPages);
            return masterPages.ToArray();
        }
    }
}