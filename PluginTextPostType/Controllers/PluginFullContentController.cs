using AutoMapper;
using MyBlogContract;
using MyBlogContract.FullContent;
using PluginTextPostType.Infrastructure;
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
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        // GET: PluginFullContent
        public ActionResult Display(IDEModelFullContent Model)
        {
            IDataStoreRecord result = _ds.GetContent(Model.PostContentId);
            VmDisplay vmodel = null;
            if (result == null)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if ( Model.PostId != result.PostId)
            {
                throw new ArgumentOutOfRangeException("PostContentId not belong postid");
            }

            if ( Model.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            vmodel = Mapper.Map<VmDisplay>(Model);
                Mapper.Map<IDataStoreRecord, VmDisplay>(result, vmodel);
                UnicodeEncoding encoding = new UnicodeEncoding();
                vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            
            return View(vmodel);
        }
    }
}