
using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
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
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public ActionResult Display(IDEModelDisplay Model)
        {
            IEnumerable<GroupVmDisplay> result = null;

            if (Model == null)
            {
                throw new NullReferenceException("Input parammeter reference must be not null");
            }

            IEnumerable<IDataStoreRecord> ds_records = _ds.GetPost(Model.PostId);
            if (ds_records == null || ds_records.Count() == 0)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.area != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }


            IList<VmDisplay> vmodel = new List<VmDisplay>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            foreach (var item in ds_records)
            {
                VmDisplay newVmDisplay = Mapper.Map<VmDisplay>(Model);
                Mapper.Map<IDataStoreRecord, VmDisplay>(item, newVmDisplay);
                newVmDisplay.Data = encoding.GetString(item.ContentData ?? encoding.GetBytes(""));
                vmodel.Add(newVmDisplay);
            }

            var group = from r in vmodel
                        group r by new { Order = r.Order, IsInGroup = r.IsInGroup }
                            into g
                        select g;

            result = group.Select(x => new GroupVmDisplay
            {
                Order = x.Key.Order,
                IsInGroup = x.Key.IsInGroup,
                VmDisplays = x.ToList()
            })
            .OrderBy(o => o.Order);
            
            return View(result);

        }

    }


   
}