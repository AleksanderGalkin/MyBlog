using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract.TagCloud
{
    public abstract class IDtoTagCloud : IDto  
    {

        abstract public int TagId { get; set; }
        abstract public string AreaName { get;  }

        abstract public string StoreModelKey { get; set; }
        abstract public string CallbackControllerName_CmdShowPostByTag { get; set; }
        abstract public string CallbackActionName_CmdShowPostByTag { get; set; }
        abstract public int TagId_CmdShowPostByTag { get; set; }

        abstract public RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin);
    }

}
