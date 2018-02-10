
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
    [Export("PluginTag", typeof(IController))]

    [InheritedExport(typeof(ITagManager)),
    ExportMetadata("Name", "PluginTag"),
    ExportMetadata("Version", "1.0"),
    ExportMetadata("ControllerName", "TagManager")
    ]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TagManagerController :ITagManager
    {

        [ImportingConstructor]
        public TagManagerController([Import( typeof(IDataStore<IDsTagModel>), AllowDefault = true)]
                                          IDataStore<IDsTagModel> DataStore) : base(DataStore)
        {
        }

        public override ActionResult Display(IDtoTag Dto_in)
        {
            if (Dto_in == null)
            {
                throw new NullReferenceException("Input parammeter reference must be not null");
            }

            if (Dto_in.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            IDsTagModel _data = _ds.GetModelByKey (Dto_in.StoreModelKey);

            VmDtoTag vmTags = Mapper.Map<IDtoTag, VmDtoTag>(Dto_in);
            Mapper.Map<IEnumerable<IDsTag>, List<VmDtoTagItem>>(_data.all_tags, vmTags.all_tags);
            Mapper.Map<IEnumerable<IDsTag>, List<VmDtoTagItem>>(_data.post_tags, vmTags.post_tags);

            vmTags.StoreModelKey = Dto_in.StoreModelKey;

            return PartialView("Display",vmTags);

        }


        //[HttpPost]
        //public ActionResult Edit(VmDtoTag Model)
        //{
        //    var route_value = Model.GetDictionary(DeDirection.ToMain);
        //    return RedirectToAction(Model.CmdShowParentPost_CallbackActionName, Model.CmdShowParentPost_CallbackControllerName, route_value);

        //}


        public ActionResult ShowTags(string StoreModelKey)
        {
            IDsTagModel _data = _ds.GetModelByKey(StoreModelKey);

            var result = _data.all_tags;

            VmDtoTag VmDtoTag = new VmDtoTag();
            Mapper.Map<IEnumerable<IDsTag>, List<VmDtoTagItem>>(_data.all_tags, VmDtoTag.all_tags);
            Mapper.Map<IEnumerable<IDsTag>, List<VmDtoTagItem>>(_data.post_tags, VmDtoTag.post_tags);

            return PartialView(VmDtoTag);
        }


        public ActionResult Call_SelectListDialog(VmDtoTag Model,
                                                [Import("PluginTag", typeof(IDataStore<SelectListStoreModel>))]IDataStore<SelectListStoreModel> SelectStore)
        {

            IDsTagModel _data = _ds.GetModelByKey(Model.StoreModelKey);

            SelectListStoreModel selectListModel = new SelectListStoreModel();

            foreach (var item in _data.all_tags)
            {
                selectListModel.all_items.Add(new SelectListStoreModelItem { Id = item.TagId, Name = item.TagName });
            }

            foreach (var item in _data.post_tags)
            {
                selectListModel.select_items.Add(new SelectListStoreModelItem { Id = item.TagId, Name = item.TagName });
            }

            SelectStore.SetModelByKey("first", selectListModel);
            SelectStore.SetPrevDto(Model); 

            DtoDisplayIn dto_out = new DtoDisplayIn { StoreModelKey = "first"
                                                        , on_change_event = "triger_reload_plugin"
                                                        , CmdGetResult_CallbackControllerName = "TagManager"
                                                        ,CmdGetResult_CallbackActionName = "Save"
                                                        ,CmdGetResult_AreaName = "PluginTag"
                                                        ,CmdGetResult_ResultLocationId = "id_tags_location"
        };

            return RedirectToAction("Display", "SelectListDialog", dto_out);
        }


        /// <summary>
        /// Вызывается при сохранении списка
        /// </summary>
        /// <returns></returns>
        public ActionResult Save([Import("PluginTag", typeof(IDataStore<SelectListStoreModel>))]IDataStore<SelectListStoreModel> SelectStore
                                            , DtoDisplayIn dto_in)
        {

            SelectListStoreModel _data_select_list = SelectStore.GetModelByKey(dto_in.StoreModelKey);
            VmDtoTag _prev_dto = SelectStore.GetPrevDto() as VmDtoTag;
            IDsTagModel _data = _ds.GetModelByKey(_prev_dto.StoreModelKey);

            var a = (from s in _data.all_tags
                     where _data_select_list.select_items.Any(si => si.Id == s.TagId)
                     select s)
                     .ToList();

            _data.post_tags.Clear();
            a.ForEach(i => _data.post_tags.Add(i));

            _ds.SetModelByKey(_prev_dto.StoreModelKey, _data);

            return RedirectToAction("ShowTags", new { StoreModelKey = _prev_dto.StoreModelKey });
        }




    }



}