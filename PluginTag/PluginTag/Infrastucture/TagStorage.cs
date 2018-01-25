using MyBlogContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTag.Infrastucture
{


    [Export(typeof(IDataStore<IDsTagModel>))]
    public class TagStorage : IDataStore<IDsTagModel>
    {
        public TagStorage() : base()
        {
        }

    }

    [Export(typeof(IDsTagModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class DsTagModel : IDsTagModel
    {
        public IList<IDsTag> all_tags { get; set; }
        public IList<IDsTag> post_tags { get; set; }



        public DsTagModel()
        {
            all_tags = new List<IDsTag>();
            post_tags = new List<IDsTag>();
        }

    }

    [Export(typeof(IDsTag))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class TagStoreModel : IDsTag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }


}