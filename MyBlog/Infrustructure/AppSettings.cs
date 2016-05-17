using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Infrastructure
{
    public static class AppSettings
    {
        public static string PluginDirectory
        { get { return "Modules"; } }
    }
}