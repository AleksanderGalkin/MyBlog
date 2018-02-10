using MyBlogContract;
using MyBlogContract.TagCloud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTagCloud.Infrastucture
{


    [Export(typeof(IDataStore<IDsTagCloudModel>))]
    public class TagStorage : IDataStore<IDsTagCloudModel>
    {
        public TagStorage() : base()
        {
        }

    }

    [Export(typeof(IDsTagCloudModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class DsTagModel : IDsTagCloudModel
    {
        public IList<IDsTagCloud> cloud_tags { get; set; }



        public DsTagModel()
        {
            cloud_tags = new List<IDsTagCloud>();
        }

    }

    [Export(typeof(IDsTagCloud))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class TagStoreModel : IDsTagCloud
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int Frequency { get; set; }

    }


}