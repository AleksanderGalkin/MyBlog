using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract
{
    public abstract class IDEModel
    {
        virtual public int Id { get; set; }
        virtual public int PostId { get; set; }

        abstract public RouteValueDictionary GetDictionary();

    }

}
