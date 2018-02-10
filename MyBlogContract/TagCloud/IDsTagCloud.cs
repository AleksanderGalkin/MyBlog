using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.TagCloud
{
    public interface IDsTagCloudModel : IStoreModel
    {
        IList<IDsTagCloud> cloud_tags { get; set; }

    }
    public interface IDsTagCloud
    {
        int TagId { get; set; }
        string TagName { get; set; }
        int Frequency { get; set; }


    }

}
