using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract.PostManage
{
    public abstract class IDEModelPostManage : IDeItemModel
    {
        abstract public string CallbackControllerName { get; set; }
        abstract public string CallbackActionName { get; set; }
        
        abstract public string List_content_insert_before_Id { get; set; }
        abstract public string Update_area_replace_Id { get; set; }
        abstract public string OnSuccessRemoveCallback { get; set; }
    }

}
