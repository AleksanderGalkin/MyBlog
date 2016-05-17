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

        Type ControllerType { get; }

        string ControllerName { get; }

        [DefaultValue("Undefined")]
        string ActionDisplayName { get; }

        [DefaultValue("Undefined")]
        string ActionModifyName { get; }

        [DefaultValue("Undefined")]
        string ActionCreateName { get; }

    }
}
