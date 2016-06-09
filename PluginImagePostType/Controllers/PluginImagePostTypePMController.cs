using AutoMapper;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
using PluginImagePostType.Infrastructure;
using PluginImagePostType.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginImagePostType.Controllers
{
    [Export("PluginImagePostType", typeof(IController)),
    ExportMetadata("Name", "PluginImagePostType"),
    ExportMetadata("Version", "1.0"),
    ExportMetadata("ControllerName", "PluginImagePostTypePM"),
    ExportMetadata("ControllerType", typeof(IPostManager)),
     ExportMetadata("ActionDisplayName", "Display"),
     ExportMetadata("ActionModifyName", "Modify"),
    ExportMetadata("ActionCreateName", "Create")]

    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PluginImagePostTypePMController : Controller
    {
        private IDataStorePostManage _ds;
        public PluginImagePostTypePMController(IDataStorePostManage DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore must be not null");
            _ds = DataStore;
        }
    

        public ViewResult Display(IDEModelPostManage Model)
        {
            
            IDataStoreRecord _ds_record = _ds.Get(Model.PostContentId);

            if (_ds_record == null)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.PostId != _ds_record.PostId)
            {
                throw new ArgumentOutOfRangeException("PostContentId not belong postid");
            }

            if (Model.area != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            VmDisplay output = Mapper.Map<VmDisplay>(Model);
            Mapper.Map<IDataStoreRecord, VmDisplay>(_ds_record, output);

            string imageBase64 = Convert.ToBase64String(_ds_record.ContentData);
            output.Data = string.Format("data:image/jpeg;base64,{0}", imageBase64);

            return View(output);
        }

        public ViewResult Create( IDEModelPostManage Model)
        {
            if (Model.area != AppSettings.PluginName)
                throw new InvalidOperationException("This plagin is not for this area.");
            if (Model.PostContentId != 0)
            {
                throw new InvalidOperationException("PostContentId not 0");
            }
            if (
                string.IsNullOrWhiteSpace(Model.CallbackActionName)
                || string.IsNullOrWhiteSpace(Model.CallbackControllerName)
                || string.IsNullOrWhiteSpace(Model.List_content_insert_before_Id)
                || string.IsNullOrWhiteSpace(Model.Update_area_replace_Id)
                || string.IsNullOrWhiteSpace(Model.OnSuccessRemoveCallback)
                )
                throw new InvalidOperationException("One or more route values are empty");
            return View();
        }

        [HttpPost]
        public ViewResult LoadFiles(HttpPostedFileBase[] files, IDEModelPostManage Model)
        {
            if (Model.area != AppSettings.PluginName)
                throw new InvalidOperationException("This plagin is not for this area.");
            if (Model.PostContentId != 0)
            {
                throw new InvalidOperationException("PostContentId not 0");
            }
            if (
                string.IsNullOrWhiteSpace(Model.CallbackActionName)
                || string.IsNullOrWhiteSpace(Model.CallbackControllerName)
                || string.IsNullOrWhiteSpace(Model.List_content_insert_before_Id)
                || string.IsNullOrWhiteSpace(Model.Update_area_replace_Id)
                || string.IsNullOrWhiteSpace(Model.OnSuccessRemoveCallback)
                )
                throw new InvalidOperationException("One or more route values are empty");
            if (files ==null || files.Count() == 0)
            {
                throw new InvalidOperationException("Files number should be  not 0");
            }

            foreach(var file in files)
            {
                int new_temp_key;
                if (_ds.Get().Count() > 0)
                {
                    new_temp_key = _ds.Get().Max(m => m.PostContentIdForNewRecords);
                    new_temp_key++;
                }
                else
                {
                    new_temp_key = 1;
                }
                IDataStoreRecord newRecord = _ds.GetNew();
                MemoryStream ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                newRecord.ContentData = ms.ToArray();
                newRecord.ContentPluginName = AppSettings.PluginName;
                newRecord.ContentPluginVersion = AppSettings.Version;
                newRecord.PostId = Model.PostId;
                newRecord.PostContentIdForNewRecords = new_temp_key;
                _ds.Create(newRecord);
            }

            return View();
        }


        public ViewResult Modify(VmDisplay Model)
        {
            VmDisplay output =null;
            if (Model!=null)
            {
                output = new VmDisplay();
                IDataStoreRecord _ds_record =
                    _ds.Get(Model.PostContentId, Model.PostContentIdForNewRecords);

                output = Mapper.Map<VmDisplay>(Model);
                Mapper.Map<IDataStoreRecord, VmDisplay>(_ds_record, output);

                string imageBase64 = Convert.ToBase64String(_ds_record.ContentData);
                output.Data = string.Format("data:image/jpeg;base64,{0}", imageBase64);
            }

            return View(output);
        }

        public ViewResult ModifyPost(VmDisplay Model)
        {
            IDataStoreRecord record = null;
            bool isRecordNew = false;
            if (Model.PostContentId != 0)
            {
                record = _ds.Get(Model.PostContentId);
                isRecordNew = false;
            }
            else
            if (Model.PostContentId == 0 && Model.PostContentIdForNewRecords != 0)
            {
                record = _ds.Get()
                    .Where(r => r.PostContentIdForNewRecords == Model.PostContentIdForNewRecords)
                    .SingleOrDefault();
                isRecordNew = false;
            }
            else
            if (Model.PostContentId == 0 && Model.PostContentIdForNewRecords == 0)
            {

                int new_temp_key;
                if (_ds.Get().Count() > 0)
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

            byte[] bytes = null;
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

        public void DeleteContent(int PostContentId, int PostContentIdForNewRecords)
        {
            _ds.Delete(PostContentId, PostContentIdForNewRecords);
            System.Web.HttpContext.Current.Session["data_store"] = _ds;

            string json_object = JsonConvert.SerializeObject(new
            {
                PostContentId = PostContentId
                                                                ,
                PostContentIdForNewRecords = PostContentIdForNewRecords
                                                                ,
                result = true
            }, Formatting.Indented);
            HttpContext.Response.Write(json_object);
        }
    }
}