using MyBlogContract;
using MyBlogContract.FullContent;
using PluginTextPostType.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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
        public ActionResult Display(IDEModelFullContent Model)
        {
            IDataStoreRecord result = _ds.Get(Model.Id);
            VmDisplay vmodel = new VmDisplay();
            vmodel.Id = result.PostContentId;
            vmodel.Comment = result.Comment;
            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));

            return View(vmodel);
        }
    }
}