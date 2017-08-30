
using AutoMapper;
using MyBlogContract;
using MyBlogContract.ContentGroup;
using MyBlogContract.PostManage;
using PluginTextPostType.Infrastructure;
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
        ExportMetadata("ControllerName", "ContentGroup"),
        ExportMetadata("ControllerType", typeof(IContentGroupDisplay)),
        ExportMetadata("ActionDisplayName", "Display")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContentGroupController :Controller, IContentGroupDisplay
    {
       
        private IDataStoreContentGroup _ds;


        [ImportingConstructor]
        public ContentGroupController(IDataStoreContentGroup DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public ActionResult Display(IDeContentGroup Model)
        {
            IEnumerable<VmContentGroup> result = null;

            if (Model == null)
            {
                throw new NullReferenceException("Input parammeter reference must be not null");
            }

            IEnumerable<IDataStoreRecord> ds_records = _ds.GetGroupContent(Model.PostId, Model.Order);

            if (ds_records == null || ds_records.Count() == 0)
            {
                // могут быть пустые посты
               // throw new InvalidOperationException("Data store not return value");
            }

            if (Model.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }


            IList<VmContentGroup> vmodel = new List<VmContentGroup>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            foreach (var item in ds_records)
            {
                VmContentGroup newVm = Mapper.Map<VmContentGroup>(Model);
                Mapper.Map<IDataStoreRecord, VmContentGroup>(item, newVm);
                newVm.Data = encoding.GetString(item.ContentData ?? encoding.GetBytes(""));
                vmodel.Add(newVm);
            }
            result = vmodel;
            return View(result);

        }

    }


   
}