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
            _ds = DataStore;
        }

        public ActionResult Display(IDEModelPostManage Model)
        {
            IDataStoreRecord result = _ds.Get(Model.Id);
            VmDisplay vmodel = new VmDisplay();

            vmodel.Id = Model.Id;
            vmodel._temporary_PostContentId = result._temporary_PostContentId;
            vmodel.PostId = result.PostId;
            vmodel.Comment = result.Comment;
            vmodel.CallbackActionName = Model.CallbackActionName;
            vmodel.CallbackControllerName = Model.CallbackControllerName;
            vmodel.OnSuccessRemoveCallback = Model.OnSuccessRemoveCallback;
            vmodel.UpdateTargetId = Model.UpdateTargetId;
            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            return View(vmodel);
        }


        public ActionResult Create(IDEModelPostManage Model)
        {
            
            VmDisplay vmodel = new VmDisplay();

            vmodel.PostId = Model.PostId;
            vmodel.CallbackActionName = Model.CallbackActionName;
            vmodel.CallbackControllerName = Model.CallbackControllerName;
            vmodel.OnSuccessRemoveCallback = Model.OnSuccessRemoveCallback;
            vmodel.UpdateTargetId = Model.UpdateTargetId;
            UnicodeEncoding encoding = new UnicodeEncoding();

            return View("Modify",vmodel);
        }

        public ActionResult Modify(VmDisplay Model)
        {
            IDataStoreRecord result = _ds.Get(Model.Id, Model._temporary_PostContentId);
            VmDisplay vmodel = new VmDisplay();
            vmodel.PostId = result.PostId;
            vmodel._temporary_PostContentId = result._temporary_PostContentId;
            vmodel.Id = result.PostContentId;
            vmodel.Comment = result.Comment;
            vmodel.CallbackActionName = Model.CallbackActionName;
            vmodel.CallbackControllerName = Model.CallbackControllerName;
            vmodel.OnSuccessRemoveCallback = Model.OnSuccessRemoveCallback;
            vmodel.UpdateTargetId = Model.UpdateTargetId;
            vmodel.data_edit_diff_flag = Model.data_edit_diff_flag;
            UnicodeEncoding encoding = new UnicodeEncoding();
            vmodel.Data = encoding.GetString(result.ContentData ?? encoding.GetBytes(""));
            
            return View(vmodel);
        }
      [HttpPost]
        public ActionResult ModifyPost(VmDisplay Model  )
        {
            IDataStoreRecord model = null;
            bool isRecordNew = false;
            if (Model.Id != 0)
            {
                model = _ds.Get(Model.Id);
                isRecordNew = false;
            }
            else
            if(Model.Id == 0 && Model._temporary_PostContentId!=0)
            {
                model = _ds.Get()
                    .Where(r=>r._temporary_PostContentId== Model._temporary_PostContentId)
                    .SingleOrDefault();
                isRecordNew = false;
            }
            else
            if (Model.Id == 0 && Model._temporary_PostContentId == 0)
            {
                model = _ds.GetNew();
                int new_temp_key = 1;
                if(_ds.Get().Count()>0)
                {
                    new_temp_key = _ds.Get().Max(m => m._temporary_PostContentId);
                }
                model._temporary_PostContentId = new_temp_key;
                Model._temporary_PostContentId = new_temp_key;
                isRecordNew = true;
            }
            model.PostId = Model.PostId;
            model.DataPluginName = AppSettings.PluginName;
            model.DataPluginVersion = AppSettings.Version;
            model.Comment = Model.Comment;
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] bytes = encoding.GetBytes(Model.Data);
            model.ContentData = bytes;

            if (isRecordNew)
            {
                _ds.Create(model);
            }
            else
            {
                _ds.Modify(model);
            }
            Model.data_edit_diff_flag = !Model.data_edit_diff_flag;
            System.Web.HttpContext.Current.Session["data_store"] = _ds;
            return View("Display", Model);
        }

        [HttpPost]
        public void DeleteContent(int Id)
        {
            _ds.Delete(Id);
            System.Web.HttpContext.Current.Session["data_store"] = _ds;
            string json_object = JsonConvert.SerializeObject(new { id = Id, result = true }, Formatting.Indented);
            HttpContext.Response.Write(json_object);
        }
        
    }
}