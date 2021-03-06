﻿
using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using PluginImagePostType.Infrastructure;
using PluginImagePostType.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PluginImagePostType.Controllers
{
    [Export("PluginImagePostType", typeof(IController)),
        ExportMetadata("Name", "PluginImagePostType"),
        ExportMetadata("Version","1.0"),
        ExportMetadata("ControllerName", "PluginImagePostTypeBand"),
        ExportMetadata("ControllerType", typeof(IBandDisplay)),
        ExportMetadata("ActionDisplayName", "Display")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PluginImagePostTypeBandController : Controller, IBandDisplay
    {
        private IDataStoreBand _ds;


        [ImportingConstructor]
        public PluginImagePostTypeBandController(IDataStoreBand DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore must be not null");
            _ds = DataStore;
        }

        public ActionResult Display(IDeGroupModelDisplay Model)
        {
            //IEnumerable<IDataStoreRecord> ds_records = _ds.GetDbPost(Model.PostId)
            //                                             .Union(_ds.GetModPost(Model.PostId));
            IEnumerable<IDataStoreRecord> ds_records = _ds.GetGroupContent(Model.PostId,Model.Order);

            if (ds_records == null || ds_records.Count() == 0)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }


            IList<VmDisplay> vmodel = new List<VmDisplay>(); 

            foreach (var item in ds_records)
            {
                VmDisplay newVmDisplay =  Mapper.Map<VmDisplay>(Model);
                Mapper.Map<IDataStoreRecord,VmDisplay>(item, newVmDisplay);
                string imageBase64 = Convert.ToBase64String(item. ContentData);
                newVmDisplay.Data = string.Format("data:image/jpeg;base64,{0}", imageBase64);
                vmodel.Add(newVmDisplay);
            }

            var group = from r in vmodel
                         group r by new  { Order = r.Order }
                         into g
                         select g;

            IEnumerable<GroupVmDisplay> result = group.Select(x => new GroupVmDisplay
                                            {
                                                Order = x.Key.Order,
                                        
                                                VmDisplays = x.ToList()
                                            })
                                            .OrderBy(o=>o.Order);
            return View(result);
        }

    }


}