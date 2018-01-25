using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace MyBlogContract
{
    /// <summary>
    /// Group of Models what used for exchanging data between host and plugins
    /// </summary>
    public abstract class IDeGroupModel
    {
        abstract public int PostId { get; set; }
        abstract public int Order { get; set; }
        abstract public string AreaName { get;  }

        abstract public RouteValueDictionary GetDictionary(DeDirection direction = DeDirection.ToPlugin);

    }

}
