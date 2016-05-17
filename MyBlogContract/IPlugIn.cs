using MyBlogContract.Band;
using MyBlogContract.PostManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlogContract
{
    [InheritedExport(typeof(IPlugIn))]
    public interface IPlugIn
    {
        string PluginName { get; }
        string Version { get; }
    }


}
