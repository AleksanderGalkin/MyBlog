
using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using PluginTextPostType.DataExchangeModels;
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
    [Export("PluginTextPostType", typeof(IController))]

    [Export("PluginTextPostType",typeof(IBandDisplay)),
        ExportMetadata("Name","PluginTextPostType"),
        ExportMetadata("Version","1.0"),
        ExportMetadata("ControllerName", "Band"),
    ]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BandController : IBandDisplay
    {
       
        private IDataStoreBand _ds;


        [ImportingConstructor]
        public BandController(IDataStoreBand DataStore): base(DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }


        public override ActionResult Display(IDeGroupBand Model)
        {
            IEnumerable<VmItemGroup> result = null;

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


            IList<VmItem> vmodel = new List<VmItem>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            foreach (var item in ds_records)
            {
                VmItem newVmDisplay = Mapper.Map<VmItem>(Model);
                Mapper.Map<IDataStoreRecord, VmItem>(item, newVmDisplay);
                newVmDisplay.Data = encoding.GetString(item.ContentData ?? encoding.GetBytes(""));
                vmodel.Add(newVmDisplay);
            }

            var group = from r in vmodel
                        group r by new { Order = Model.Order}
                            into g
                        select g;

            result = group.Select(x => new VmItemGroup
            {
                Order = x.Key.Order,
                VmItems = x.ToList()
            })
            .OrderBy(o => o.Order);
            
            return View(result);

        }

        public override string GetPostUrl(IDeGroupBand Model)
        {
            DeItem dte = new DeItem();
            dte.PostId = Model.PostId;
            dte.PostId_CmdShowPostView = Model.PostId;

           // dte.AreaName = Model.AreaName;
            string actionName = Model.CallbackActionName_CmdShowPostView;
            string controllerName = Model.CallbackControllerName_CmdShowPostView;
            string action_link = Url.Action(actionName, controllerName, dte.GetDictionary(MyBlogContract.DeDirection.ToMain), null);
            return action_link;
        }
    }


   
}