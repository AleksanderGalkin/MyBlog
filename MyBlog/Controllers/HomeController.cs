using MyBlog.CrossConserns.Exceptions;
using MyBlog.DomenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    
    public class HomeController : Controller
    {
        [ExceptionCatcherAttribute]
        public ActionResult Index()
        {
            var q= 10 -10;
            var s = 10 / q;
            SiteDBContext db = new SiteDBContext();
            var r = from a in db.Users
                    select a;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}