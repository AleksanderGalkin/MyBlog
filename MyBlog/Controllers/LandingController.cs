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
            Tuple<LandingRegisterViewModel, LandingLoginViewModel> result =
             new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(new LandingRegisterViewModel(), new LandingLoginViewModel());
            return View("Index", result);
        }
        // POST: Landing/Register
        [HttpPost]
        public ActionResult Register(LandingRegisterViewModel Model) //(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = Model.EmailReg;
                _unitOfWork.db.Users.Add(user);
                return RedirectToAction("Index");
            }
            Tuple < LandingRegisterViewModel, LandingLoginViewModel > result =
              new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(Model,new LandingLoginViewModel()) ;
            return View("Index", result);
        }

        // POST: Landing/Login
        [HttpPost]
        public ActionResult Login( LandingLoginViewModel Model) //(FormCollection collection)
        {
            if ( ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = Model.EmailLog;
                _unitOfWork.db.Users.Add(user);
                return RedirectToAction("Index");
            }
            return View("Index", Model);
        }

    }
}
