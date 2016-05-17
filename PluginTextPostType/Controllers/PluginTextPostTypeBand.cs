
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using PluginTextPostType.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PluginTextPostType.Controllers
{
    [Export("PluginTextPostType",typeof(IController)),
        ExportMetadata("Name","PluginTextPostType"),
        ExportMetadata("Version","1.0"),
        ExportMetadata("ControllerName", "PluginTextPostTypeBand"),
        ExportMetadata("ControllerType", typeof(IBandDisplay)),
        ExportMetadata("ActionDisplayName", "Display")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PluginTextPostTypeBandController :Controller, IBandDisplay
    {
        private IDataStoreBand _ds;


        [ImportingConstructor]
        public PluginTextPostTypeBandController(IDataStoreBand DataStore)
        {
            _ds = DataStore;
        }

        public ActionResult Display(IDEModelDisplay Model)
        {
            IDataStoreRecord result = _ds.Get(Model.Id);
            VmDisplay vmodel= new VmDisplay();
            vmodel.Id = result.PostContentId;
            vmodel.Comment = result.Comment;
            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            
            return View(vmodel);
        }

    }


   
}