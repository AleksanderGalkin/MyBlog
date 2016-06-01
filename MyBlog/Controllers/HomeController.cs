using MyBlog.Models;
using MyBlog.ViewModels;
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
        ApplicationDbContext  db = new ApplicationDbContext();
        public ActionResult Index()
        {
            
            IEnumerable<LoginVm> model = new List<LoginVm> { new LoginVm { EmailLog = "a@a" }
                                                                                          , new LoginVm { EmailLog = "b@b" }};
                                                                                      
           // user
            return View(model);
        }

        // GET: /Movies/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginVm user = new LoginVm { EmailLog = "a@a" };
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            ApplicationUser model = new ApplicationUser();
           // model.DateOfMailSended = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fullname,Mail,IsMailSended,DateOfMailSended,IsDeliveryError,IsUserBack,IsUserChangePassword,IsUserConfirmRegistration")] ApplicationUser Model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(Model);
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
            ApplicationUser model = db.Users.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ([Bind(Include = "Id,Fullname,Mail,IsMailSended,DateOfMailSended,IsDeliveryError,IsUserBack,IsUserChangePassword,IsUserConfirmRegistration,RowVer")]ApplicationUser model)
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
            ApplicationUser model = db.Users.Find(id);
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
            ApplicationUser model = db.Users.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            db.Users.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Галкин Александр Петрович";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "В случае, если Вы хотите что-то мне сообщить:";

            return View("Contact");
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