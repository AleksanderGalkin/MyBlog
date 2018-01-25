using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract
{
    public interface IMetadata
    {
        string Name { get; }
        string Version { get; }
        string ControllerName { get; }




    }
}
