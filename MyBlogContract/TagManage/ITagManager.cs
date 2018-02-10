using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlogContract.TagManage
{
    /*   Example
    [InheritedExport("<ContractName>", typeof(<classname>)),
    ExportMetadata("Name", "<PluginName>"),
    ExportMetadata("Version", "<PluginVersion>"),
    ExportMetadata("ControllerName", "<ControllerName>"),
    ]
    */
    public abstract class ITagManager : Controller
    {
        protected IDataStore<IDsTagModel> _ds;


        protected ITagManager( IDataStore<IDsTagModel> DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public abstract ActionResult Display(IDtoTag Model);

    }

}
