using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogContract.Band
{
    public abstract class IDeGroupBand : IDeGroupModel
    {
        abstract public string CallbackControllerName_CmdShowFullContent { get; set; }
        abstract public string CallbackActionName_CmdShowFullContent { get; set; }
        abstract public int ContentId_CmdShowFullContent { get; set; }

        abstract public string CallbackControllerName_CmdShowPostView { get; set; }
        abstract public string CallbackActionName_CmdShowPostView { get; set; }
        abstract public int PostId_CmdShowPostView { get; set; }
    }

}
