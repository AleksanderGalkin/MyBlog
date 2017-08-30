using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.ContentGroup
{
    public interface IContentGroupDisplay : IController
    {
        ActionResult Display(IDeContentGroup Model);
    }

}
