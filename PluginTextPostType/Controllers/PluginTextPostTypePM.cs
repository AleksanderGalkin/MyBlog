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
            IDataStoreRecord result = _ds.GetContent(Model.PostContentId);

            if (result == null)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.PostId != result.PostId)
            {
                throw new ArgumentOutOfRangeException("PostContentId not belong postid");
            }

            if (Model.AreaName != AppSettings.PluginName)
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
            if (Model.AreaName != AppSettings.PluginName)
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
            IDataStoreRecord result = _ds.GetContent(Model.PostContentId, Model.tempPostContentId);

            VmDisplay vmodel = Mapper.Map<VmDisplay>(Model);

            Mapper.Map<IDataStoreRecord, VmDisplay>(result, vmodel);

            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            
            return View(vmodel);
        }

        [HttpPost]
        public ActionResult ModifyPost(VmDisplay Model)
        {

            IDataStoreRecord newRecord = _ds.GetNew();
            newRecord = Mapper.Map<VmDisplay, IDataStoreRecord>(Model, newRecord);
            newRecord.ContentPluginName = AppSettings.PluginName;
            newRecord.ContentPluginVersion = AppSettings.Version;
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] bytes = encoding.GetBytes(Model.Data);
            newRecord.ContentData = bytes;
            _ds.Modify(newRecord);
            Model.data_edit_diff_flag = !Model.data_edit_diff_flag;
            return View("Display", Model);
        }
        //[HttpPost]
        //public ActionResult ModifyPost(VmDisplay Model  )
        //{
        //    IDataStoreRecord record = null;
        //    bool isRecordNew = false;
        //    if (Model.PostContentId != 0)
        //    {
        //        record = _ds.GetContent(Model.PostContentId);
        //        isRecordNew = false;
        //    }
        //    else
        //    if(Model.PostContentId == 0 && Model.tempPostContent!=0)
        //    {
        //        record = _ds.GetAllContents()
        //            .Where(r=>r.tempPostContentId== Model.tempPostContent)
        //            .SingleOrDefault();
        //        isRecordNew = false;
        //    }
        //    else
        //    if (Model.PostContentId == 0 && Model.tempPostContent == 0)
        //    {
                
        //        int new_temp_key;
        //        if(_ds.GetAllContents().Count()>0)
        //        {
        //            new_temp_key = _ds.GetAllContents().Max(m => m.tempPostContentId);
        //            new_temp_key++;
        //        }
        //        else
        //        {
        //            new_temp_key = 1;
        //        }
        //        record = _ds.GetNew();
        //        record.tempPostContentId = new_temp_key;
        //        Model.tempPostContent = new_temp_key;
        //        isRecordNew = true;
        //    }
        //    record.PostId = Model.PostId;
        //    record.ContentPluginName = AppSettings.PluginName;
        //    record.ContentPluginVersion = AppSettings.Version;
        //    record.Comment = Model.Comment;
        //    UnicodeEncoding encoding = new UnicodeEncoding();
        //    byte[] bytes = encoding.GetBytes(Model.Data);
        //    record.ContentData = bytes;

        //    if (isRecordNew)
        //    {
        //       // _ds.Create(record);
        //    }
        //    else
        //    {
        //        _ds.Modify(record);
        //    }
        //    Model.data_edit_diff_flag = !Model.data_edit_diff_flag;
   
        //    System.Web.HttpContext.Current.Session["data_store"] = _ds;
        //    return View("Display", Model);
        //}

        [HttpPost]
        public void DeleteContent(int PostContentId, int tempPostContentId)
        {
            _ds.Delete(PostContentId, tempPostContentId);
           // System.Web.HttpContext.Current.Session["data_store"] = _ds;

            string json_object = JsonConvert.SerializeObject(new { PostContentId = PostContentId
                                                                , tempPostContentId = tempPostContentId
                                                                , result = true }, Formatting.Indented);
            HttpContext.Response.Write(json_object);
        }
        
    }
}