using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract.TagManage
{
    public abstract class IDtoTag : IDto  
    {

        abstract public int TagId { get; set; }
        abstract public int PostId { get; set; }
        abstract public string AreaName { get;  }

        abstract public string StoreModelKey { get; set; }
        abstract public string CallbackControllerName_CmdShowPostByTag { get; set; }
        abstract public string CallbackActionName_CmdShowPostByTag { get; set; }
        abstract public int TagId_CmdShowPostByTag { get; set; }


        abstract public string CmdShowParentPost_CallbackControllerName { get; set; }
        abstract public string CmdShowParentPost_CallbackActionName { get; set; }
        abstract public int CmdShowParentPost_PostId { get; set; }


        abstract public RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin);
    }

}
