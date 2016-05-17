using MyBlogContract.FullContent;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginTextPostType.Controllers
{
    [Export("PluginTextPostType", typeof(IController)),
    ExportMetadata("Name", "PluginTextPostType"),
    ExportMetadata("Version", "1.0"),
    ExportMetadata("ControllerName", "PluginFullContent"),
    ExportMetadata("ControllerType", typeof(IFullContent)),
    ExportMetadata("ActionDisplayName", "Display")]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class PluginFullContentController : Controller
    {
        private IDataStoreFullContent _ds;


        [ImportingConstructor]
        public PluginFullContentController(IDataStoreFullContent DataStore)
        {
            _ds = DataStore;
        }

        // GET: PluginFullContent
        public ActionResult Display(int Id)
        {
            return View();
        }
    }
}