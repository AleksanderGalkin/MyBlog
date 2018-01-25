using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.PostManage
{
    /*   Example
    [InheritedExport("<ContractName>", typeof(<classname>)),
    ExportMetadata("Name", "<PluginName>"),
    ExportMetadata("Version", "<PluginVersion>"),
    ExportMetadata("ControllerName", "<ControllerName>"),
    ]
    */
    public abstract class IPostManager : Controller
    {
        protected IPostManager(IDataStorePostManage DataStore)
        { }

        public abstract ActionResult Display(IDEModelPostManage Model);
        public abstract ActionResult Create(IDEModelPostManage Model);
        public abstract ActionResult Modify(IDEModelPostManage Model);

    }

}
