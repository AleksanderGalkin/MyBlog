using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract
{
    /// <summary>
    /// Model what used for exchanging data between host and plugins
    /// </summary>
    public abstract class IDEModel
    {
        abstract public int PostId { get; set; }
        abstract public int PostContentId { get; set; }
        abstract public string area { get; set; }

        abstract public RouteValueDictionary GetDictionary();

    }

}
