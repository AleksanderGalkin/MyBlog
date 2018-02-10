
using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using MyBlogContract.TagManage;
using PluginTag.DataExchangeModels;
using PluginTag.Infrastructure;
using PluginTag.SelectListData;
using PluginTag.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PluginTag.Controllers
{

    public abstract class ISelectListDialog : Controller
    {


        protected ISelectListDialog(IDataStore<SelectListStoreModel> DataStore)
        {

        }

        public abstract ActionResult Display(DtoDisplayIn dto_in);

    }

    [Export("PluginTag", typeof(IController))]

    [InheritedExport("PluginTag", typeof(ISelectListDialog)),
       ExportMetadata("Name", "PluginTag"),
       ExportMetadata("Version", "1.0"),
       ExportMetadata("ControllerName", "SelectListDialog")
       ]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SelectListDialogController : ISelectListDialog
    {
          private IDataStore<SelectListStoreModel> _ds;

        [ImportingConstructor]
        public SelectListDialogController([Import("PluginTag", typeof(IDataStore<SelectListStoreModel>))]
                                                IDataStore<SelectListStoreModel> DataStore)
            :base(DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
             _ds = DataStore;
        }

        public override ActionResult Display(DtoDisplayIn dto_in)
        {
            SelectListStoreModel _data = _ds.GetModelByKey(dto_in.StoreModelKey);

            VmDtoDisplayIn model_out = Mapper.Map< SelectListStoreModel , VmDtoDisplayIn> (_data);
            Mapper.Map<DtoDisplayIn, VmDtoDisplayIn>(dto_in, model_out);

            return PartialView("Display", model_out);

        }


        [HttpPost]
        public ActionResult Display(VmDtoDisplayIn Model_out)
        {
            SelectListStoreModel _data = _ds.GetModelByKey(Model_out.StoreModelKey);


            _data.select_items = Model_out.select_items;

            if (Model_out.CmdGetResult_CallbackActionName == null || Model_out.CmdGetResult_CallbackControllerName == null)
            {
                return null;
            }
            else
            {
                return RedirectToAction(Model_out.CmdGetResult_CallbackActionName,
                                        Model_out.CmdGetResult_CallbackControllerName,
                                        Model_out
                                        );
            }


        }




    }



}