using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{

    public class LandingController : AbstractController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public LandingController(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
         
        }
        //public LandingController(IUnitOfWork UnitOfWork, ApplicationUserManager userManager, ApplicationSignInManager signInManager) :base(UnitOfWork)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Landing
        public ActionResult Index()
        {
            Tuple<LandingRegisterViewModel, LandingLoginViewModel> result =
             new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(new LandingRegisterViewModel(), new LandingLoginViewModel());
            return View("Index", result);
        }
        // POST: Landing/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(LandingRegisterViewModel Model) //(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Model.FullName, Email = Model.EmailReg, isNotificationAllowed=Model.NotifyMe };
                Random random = new Random(DateTime.Now.Day);
                StringBuilder strBuilder = new StringBuilder("!164");
                for (int i = 0; i < 3; i++)
                {
                    strBuilder.Append((char)random.Next(65, 90));
                    strBuilder.Append((char)random.Next(97, 122));
                }
                string temporaryPassword = strBuilder.ToString();
                var result = await UserManager.CreateAsync(user, temporaryPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            Tuple<LandingRegisterViewModel, LandingLoginViewModel> badModel =
              new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(Model, new LandingLoginViewModel());
            return View("Index", badModel);



            //if (ModelState.IsValid)
            //{
            //    ApplicationUser user = new ApplicationUser();
            //    user.Email = Model.EmailReg;
            //    _unitOfWork.db.Users.Add(user);
            //    return RedirectToAction("Index");
            //}
            //Tuple < LandingRegisterViewModel, LandingLoginViewModel > result =
            //  new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(Model,new LandingLoginViewModel()) ;
            //return View("Index", result);
        }

        // POST: Landing/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


    }
}
