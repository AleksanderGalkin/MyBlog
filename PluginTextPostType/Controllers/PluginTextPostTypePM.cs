using AutoMapper;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
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
    [Export("PluginTextPostType", typeof(IController)),
        ExportMetadata("Name", "PluginTextPostType"),
        ExportMetadata("Version", "1.0"),
        ExportMetadata("ControllerName", "PluginTextPostTypePM"),
        ExportMetadata("ControllerType", typeof(IPostManager)),
         ExportMetadata("ActionDisplayName", "Display"),
         ExportMetadata("ActionModifyName", "Modify"),
        ExportMetadata("ActionCreateName", "Create")]
    
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PluginTextPostTypePMController : Controller, IPostManager

    {
        private IDataStorePostManage _ds;
        
        [ImportingConstructor]
        public PluginTextPostTypePMController(IDataStorePostManage DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public ActionResult Display(IDEModelPostManage Model)
        {
            IDataStoreRecord result = _ds.Get(Model.PostContentId);

            if (result == null)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.PostId != result.PostId)
            {
                throw new ArgumentOutOfRangeException("PostContentId not belong postid");
            }

            if (Model.area != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            

            VmDisplay vmodel = Mapper.Map<VmDisplay>(Model);

            Mapper.Map<IDataStoreRecord,VmDisplay>(result, vmodel);

            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            return View(vmodel);
        }


        public ActionResult Create(IDEModelPostManage Model)
        {
            if (Model.area != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            if (Model.PostContentId != 0)
            {
                throw new InvalidOperationException("PostContentId not 0");
            }

            if(string.IsNullOrWhiteSpace(Model.CallbackControllerName )
                || string.IsNullOrWhiteSpace(Model.CallbackActionName )
                || string.IsNullOrWhiteSpace(Model.List_content_insert_before_Id)
                || string.IsNullOrWhiteSpace(Model.Update_area_replace_Id)
                || string.IsNullOrWhiteSpace(Model.OnSuccessRemoveCallback)
                )
            {
                throw new InvalidOperationException("Route variables contain empty values ");
            }


            VmDisplay vmodel = Mapper.Map<VmDisplay>(Model);

            return View("Modify",vmodel);
        }

        public ActionResult Modify(VmDisplay Model)
        {
            IDataStoreRecord result = _ds.Get(Model.PostContentId, Model.PostContentIdForNewRecords);

            VmDisplay vmodel = Mapper.Map<VmDisplay>(Model);

            Mapper.Map<IDataStoreRecord, VmDisplay>(result, vmodel);

            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            
            return View(vmodel);
        }
      [HttpPost]
        public ActionResult ModifyPost(VmDisplay Model  )
        {
            IDataStoreRecord record = null;
            bool isRecordNew = false;
            if (Model.PostContentId != 0)
            {
                record = _ds.Get(Model.PostContentId);
                isRecordNew = false;
            }
            else
            if(Model.PostContentId == 0 && Model.PostContentIdForNewRecords!=0)
            {
                record = _ds.Get()
                    .Where(r=>r.PostContentIdForNewRecords== Model.PostContentIdForNewRecords)
                    .SingleOrDefault();
                isRecordNew = false;
            }
            else
            if (Model.PostContentId == 0 && Model.PostContentIdForNewRecords == 0)
            {
                
                int new_temp_key;
                if(_ds.Get().Count()>0)
                {
                    new_temp_key = _ds.Get().Max(m => m.PostContentIdForNewRecords);
                    new_temp_key++;
                }
                else
                {
                    new_temp_key = 1;
                }
                record = _ds.GetNew();
                record.PostContentIdForNewRecords = new_temp_key;
                Model.PostContentIdForNewRecords = new_temp_key;
                isRecordNew = true;
            }
            record.PostId = Model.PostId;
            record.ContentPluginName = AppSettings.PluginName;
            record.ContentPluginVersion = AppSettings.Version;
            record.Comment = Model.Comment;
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] bytes = encoding.GetBytes(Model.Data);
            record.ContentData = bytes;

            if (isRecordNew)
            {
                _ds.Create(record);
            }
            else
            {
                _ds.Modify(record);
            }
            Model.data_edit_diff_flag = !Model.data_edit_diff_flag;
   
            System.Web.HttpContext.Current.Session["data_store"] = _ds;
            return View("Display", Model);
        }

        [HttpPost]
        public void DeleteContent(int PostContentId, int PostContentIdForNewRecords)
        {
            _ds.Delete(PostContentId, PostContentIdForNewRecords);
            System.Web.HttpContext.Current.Session["data_store"] = _ds;

            string json_object = JsonConvert.SerializeObject(new { PostContentId = PostContentId
                                                                , PostContentIdForNewRecords = PostContentIdForNewRecords
                                                                , result = true }, Formatting.Indented);
            HttpContext.Response.Write(json_object);
        }
        
    }
}