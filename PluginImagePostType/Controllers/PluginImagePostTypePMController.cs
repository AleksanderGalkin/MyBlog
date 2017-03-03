using AutoMapper;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
using PluginImagePostType.Infrastructure;
using PluginImagePostType.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        [ImportingConstructor]
        public PluginImagePostTypePMController(IDataStorePostManage DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore must be not null");
            _ds = DataStore;
        }
    

        public ViewResult Display(IDEModelPostManage Model)
        {
            
            IDataStoreRecord _ds_record = _ds.GetContent(Model.PostContentId);

            if (_ds_record == null)
            {
                throw new InvalidOperationException("Data store not return value");
            }

            if (Model.PostId != _ds_record.PostId)
            {
                throw new ArgumentOutOfRangeException("PostContentId not belong postid");
            }

            if (Model.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            VmDisplay output = Mapper.Map<VmDisplay>(Model);
            Mapper.Map<IDataStoreRecord, VmDisplay>(_ds_record, output);

            string imageBase64 = Convert.ToBase64String(_ds_record.ContentData);
            output.Data = string.Format("data:image/jpeg;base64,{0}", imageBase64);

            return View(output);
        }

        public ViewResult Create(IDEModelPostManage Model)
        {
            if (Model.AreaName != AppSettings.PluginName)
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
            VmDisplay VModel = Mapper.Map<IDEModelPostManage, VmDisplay>(Model);
            return View("Edit", VModel);
        }

        //[HttpPost]
        //public ViewResult LoadFiles(HttpPostedFileBase[] files, IDEModelPostManage Model)
        //{
        //    if (Model.AreaName != AppSettings.PluginName)
        //        throw new InvalidOperationException("This plagin is not for this area.");
        //    if (Model.PostContentId != 0)
        //    {
        //        throw new InvalidOperationException("PostContentId not 0");
        //    }
        //    if (
        //        string.IsNullOrWhiteSpace(Model.CallbackActionName)
        //        || string.IsNullOrWhiteSpace(Model.CallbackControllerName)
        //        || string.IsNullOrWhiteSpace(Model.List_content_insert_before_Id)
        //        || string.IsNullOrWhiteSpace(Model.Update_area_replace_Id)
        //        || string.IsNullOrWhiteSpace(Model.OnSuccessRemoveCallback)
        //        )
        //        throw new InvalidOperationException("One or more route values are empty");
        //    if (files ==null || files.Count() == 0)
        //    {
        //        throw new InvalidOperationException("Files number should be  not 0");
        //    }

        //    foreach(var file in files)
        //    {

        //        IDataStoreRecord newRecord = _ds.GetNew();
        //        file.InputStream.Position = 0;
        //        MemoryStream ms = new MemoryStream();
        //        file.InputStream.CopyTo(ms);
        //        newRecord.ContentData = ms.ToArray();



        //        newRecord.ContentPluginName = AppSettings.PluginName;
        //        newRecord.ContentPluginVersion = AppSettings.Version;
        //        newRecord.PostId = Model.PostId;

        //        _ds.Modify(newRecord);
        //    }

        //    return View();
        //}

        [HttpPost] //LoadFile
            public void LoadFile(List<HttpPostedFileBase> files, string comment) 
        {
                
            string arViewResults = "";
          
            foreach (var file in files)
            {
                VmDisplay vModel = new VmDisplay();
                vModel.Comment = comment;
                
                IDataStoreRecord newRecord = _ds.GetNew();
                
                newRecord = Mapper.Map<VmDisplay, IDataStoreRecord>(vModel, newRecord);
                newRecord.ContentPluginName = AppSettings.PluginName;
                newRecord.ContentPluginVersion = AppSettings.Version;
                MemoryStream s = new MemoryStream();
                file.InputStream.Position = 0;
                file.InputStream.CopyTo(s);
                newRecord.ContentData = s.ToArray();
                _ds.Modify(newRecord);

                switch (file.ContentType)
                {
                    case "image/jpeg":
                        
                        vModel.Comment = comment;
                        
                        file.InputStream.Position = 0;
                        file.InputStream.CopyTo(s);
                        string imageBase64 = Convert.ToBase64String(s.ToArray());
                        vModel.Data = string.Format("data:image/jpeg;base64,{0}", imageBase64);
                
                        break;
                    case "video":
                        break;
                    default:
                        throw new NotImplementedException("Неизвестный тип файла");
                }
                arViewResults = arViewResults + this.RenderPartialViewToString("Display", vModel); // View("AttachedContent", newContent);
            }


                string json_object = JsonConvert. SerializeObject(arViewResults,Formatting.Indented);
                HttpContext.Response.Write(arViewResults);
            }



        public ViewResult Modify(VmDisplay Model)
        {
            VmDisplay output =null;
            if (Model!=null)
            {
                output = new VmDisplay();
                IDataStoreRecord _ds_record =
                    _ds.GetContent(Model.PostContentId, Model.tempPostContentId);

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
                record = _ds.GetContent(Model.PostContentId);
                isRecordNew = false;
            }
            else
            if (Model.PostContentId == 0 && Model.tempPostContentId != 0)
            {
                record = _ds.GetAllContents()
                    .Where(r => r.tempPostContentId == Model.tempPostContentId)
                    .SingleOrDefault();
                isRecordNew = false;
            }
            else
            if (Model.PostContentId == 0 && Model.tempPostContentId == 0)
            {

                record = _ds.GetNew();
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
                _ds.Modify(record);
            }
            else
            {
                _ds.Modify(record);
            }
            Model.data_edit_diff_flag = !Model.data_edit_diff_flag;


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

    public static class ControllerExtensions
    {
        /// <summary>
        /// Renders the specified partial view to a string.
        /// </summary>
        /// <param name="controller">The current controller instance.</param>
        /// <param name="viewName">The name of the partial view.</param>
        /// <param name="model">The model.</param>
        /// <returns>The partial view as a string.</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                // Find the partial view by its name and the current controller context.
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                // Create a view context.
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                // Render the view using the StringWriter object.
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

}

