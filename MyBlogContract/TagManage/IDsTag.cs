using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract
{
    public interface IDsTagModel : IStoreModel
    {
        IList<IDsTag> all_tags { get; set; }
        IList<IDsTag> post_tags { get; set; }

    }
    public interface IDsTag 
    {
        int TagId { get; set; }
        string TagName { get; set; }


    }

}
