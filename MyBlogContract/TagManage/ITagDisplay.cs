using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.TagManage
{

    /*   Example
    [InheritedExport("<ContractName>", typeof(<class>)),
    ExportMetadata("Name", "<PluginName>"),
    ExportMetadata("Version", "<PluginVersion>"),
    ExportMetadata("ControllerName", "<ControllerName>"),
    ]
    */
    public abstract class ITagDisplay : Controller
    {
        protected IDataStore<IDsTagModel> _ds;

        protected ITagDisplay(IDataStore<IDsTagModel> DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public abstract ActionResult Display(IDtoTag Model);
    }

}
