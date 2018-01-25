
using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using MyBlogContract.TagManage;
using PluginTag.Infrastructure;
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

    [InheritedExport( typeof(ITagDisplay)),
    ExportMetadata("Name", "PluginTag"),
    ExportMetadata("Version","1.0"),
    ExportMetadata("ControllerName", "TagDisp"),
    ]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TagDispController : ITagDisplay
    {
       
        [ImportingConstructor]
        public TagDispController([Import( typeof(IDataStore<IDsTagModel>))]
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

            IDsTagModel _data = _ds.GetModelByKey(Dto_in.StoreModelKey);

            if (_data == null)
            {
                throw new InvalidOperationException("Data not found");
            }

            VmDtoTag vmTags = Mapper.Map<IDtoTag, VmDtoTag>(Dto_in);
            Mapper.Map<IEnumerable<IDsTag>, List<VmDtoTagItem>>(_data.post_tags, vmTags.post_tags);

            vmTags.StoreModelKey = Dto_in.StoreModelKey;


            return PartialView(vmTags);

        }


    }


   
}