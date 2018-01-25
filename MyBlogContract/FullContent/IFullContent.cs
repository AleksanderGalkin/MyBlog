using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.FullContent
{
    /*   Example
    [InheritedExport("<ContractName>", typeof(<classname>)),
    ExportMetadata("Name", "<PluginName>"),
    ExportMetadata("Version", "<PluginVersion>"),
    ExportMetadata("ControllerName", "<ControllerName>"),
    ]
*/
    public abstract class IFullContent : Controller
    {
    }

}
