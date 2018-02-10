using MyBlogContract.TagCloud;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlogContract;
using PluginTagCloud.Infrastructure;
using AutoMapper;
using PluginTagCloud.ViewModels;

namespace PluginTagCloud.Controllers
{
    [Export("PluginTagCloud", typeof(IController))]

    [Export( typeof(ITagCloudDisplay)),
    ExportMetadata("Name", "PluginTagCloud"),
    ExportMetadata("Version", "1.0"),
    ExportMetadata("ControllerName", "Board"),
]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class BoardController : ITagCloudDisplay
    {
        [ImportingConstructor]
        public BoardController([Import( typeof(IDataStore<IDsTagCloudModel>))]
                                          IDataStore<IDsTagCloudModel> DataStore) : base(DataStore)
        {
        }

        // GET: Board
        public override ActionResult Display(IDtoTagCloud Dto_in)
        {
            if (Dto_in == null)
            {
                throw new NullReferenceException("Input parammeter reference must be not null");
            }

            if (Dto_in.AreaName != AppSettings.PluginName)
            {
                throw new InvalidOperationException("Area not this plugin");
            }

            IDsTagCloudModel _data = _ds.GetModelByKey(Dto_in.StoreModelKey);

            if (_data == null)
            {
                throw new InvalidOperationException("Data not found");
            }

            VmDtoTagCloud vmTagCloud = Mapper.Map<IDtoTagCloud, VmDtoTagCloud>(Dto_in);
            Mapper.Map<IEnumerable<IDsTagCloud>, List<VmDtoTagCloudItem>>(_data.cloud_tags, vmTagCloud.cloud_tags);

            vmTagCloud.StoreModelKey = Dto_in.StoreModelKey;

            return PartialView(vmTagCloud);
        }
    }
}