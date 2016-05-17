using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract.PostManage
{
    public abstract class IDEModelPostManage : IDEModel
    {
        virtual public string CallbackControllerName { get; set; }
        virtual public string CallbackActionName { get; set; }
        virtual public string UpdateTargetId { get; set; }
        virtual public string OnSuccessRemoveCallback { get; set; }
    }

}
