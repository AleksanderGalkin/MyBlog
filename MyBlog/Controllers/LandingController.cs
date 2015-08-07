using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class LandingController : AbstractController
    {
        public LandingController(IUnitOfWork UnitOfWork) :base(UnitOfWork)
        { }
        // GET: Landing
        public ActionResult Index()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Index(LandingViewModel Model)
        {
            if (ModelState.IsValid) { }
            return RedirectToAction("Index");
        }

        // POST: Landing/Register
        [HttpPost]
        public ActionResult Register(Tuple<MyBlog.ViewModels.RegisterViewModel, MyBlog.ViewModels.LoginViewModel> Model) //(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = Model.Item1.Email;
                _unitOfWork.db.Users.Add(user);
                return RedirectToAction("Index");
            }
            return View("Index",Model);
        }

        // POST: Landing/Login
        [HttpPost]
        public ActionResult Login(Tuple<MyBlog.ViewModels.RegisterViewModel, MyBlog.ViewModels.LoginViewModel> Model) //(FormCollection collection)
        {
            if ( ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = Model.Item1.Email;
                _unitOfWork.db.Users.Add(user);
                return RedirectToAction("Index");
            }
            return View("Index", Model);
        }

    }
}
