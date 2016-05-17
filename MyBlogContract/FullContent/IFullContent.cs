using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.FullContent
{
    public interface IFullContent : IController
    {
        ActionResult Display(IDEModelFullContent Model);
    }

}
