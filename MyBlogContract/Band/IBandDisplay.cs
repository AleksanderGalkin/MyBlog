using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.Band
{
    public interface IBandDisplay : IController
    {
        ActionResult Display(IDeGroupBand Model);
        string GetPostUrl(IDeGroupBand Model);
    }

}
