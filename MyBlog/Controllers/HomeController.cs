using MyBlog.CrossConserns.Exceptions;
using MyBlog.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        // GET: /Movies/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration movie = db.Registrations.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult Create()
        {
            Registration model = new DomainModels.Registration();
            model.DateOfMailSended = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fullname,Mail,IsMailSended,DateOfMailSended,IsDeliveryError,IsUserBack,IsUserChangePassword,IsUserConfirmRegistration")] Registration Model)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(Model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Model);
        }

        // GET: /Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration model = db.Registrations.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ([Bind(Include = "Id,Fullname,Mail,IsMailSended,DateOfMailSended,IsDeliveryError,IsUserBack,IsUserChangePassword,IsUserConfirmRegistration,RowVer")]Registration model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration model = db.Registrations.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmation(int? id)
        {
            if (id == null)
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration model = db.Registrations.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            db.Registrations.Remove(model);
            db.SaveChanges();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}