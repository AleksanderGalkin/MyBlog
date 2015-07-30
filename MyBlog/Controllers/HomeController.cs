using MyBlog.CrossConserns.Exceptions;
using MyBlog.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    
    public class HomeController : Controller
    {
        SiteDBContext db = new SiteDBContext();
        [ExceptionCatcherAttribute]
        public ActionResult Index()
        {
            
            IEnumerable<Registration> model = db.Registrations.Select(x=>x);
            return View(model);
        }

        public ActionResult Create()
        {
            Registration model = new DomainModels.Registration();
            model.DateOfMailSended = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(Registration Model)
        {
            db.Registrations.Add(Model);
            try { db.SaveChanges();}
            catch(Exception e)
            { }
            
            return RedirectToAction("Index");
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