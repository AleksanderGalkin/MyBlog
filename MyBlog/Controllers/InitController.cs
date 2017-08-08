using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Helpers;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Export(typeof(IController)),
        ExportMetadata("Name", ""),
        ExportMetadata("Version", ""),
        ExportMetadata("ControllerName", "Init"),
        ExportMetadata("ControllerType", typeof(IController))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InitController : AbstractController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [ImportingConstructor]
        public InitController(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
         
        }

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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Init
       
        public ActionResult Welcome()
        {
            Tuple<RegisterVm, LoginVm> result =
             new Tuple<RegisterVm, LoginVm>(new RegisterVm(), new LoginVm());
            return View("Welcome", result);
        }
        // POST: Init/Register

        [HttpPost]
        [AllowAnonymous]
        [HandleError(
            ExceptionType = typeof(HttpAntiForgeryException),
            View = "AntiForgeryTokenException")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVm Model) 
        {
            Tuple<RegisterVm, LoginVm> badModel;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Model.EmailReg,  Email = Model.EmailReg };
                IdentityUserClaim _claim = new Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim();
                _claim.ClaimType = "FullName";
                _claim.ClaimValue = Model.FullName;
                _claim.UserId = user.Id;
                user.Claims.Add(_claim);
                ViewBag.FullName = Model.FullName;
                ViewBag.Email = user.Email;
                
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
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmailAndChangePassword", "Init", new { userId = System.Web.HttpUtility.UrlEncode(user.Id), code = System.Web.HttpUtility.UrlEncode(code) }, protocol: Request.Url.Scheme);
                    try
                    {
                        ViewBag.Logo = Url.Content("~/Content/images/init/Logo.png", true);
                        ViewBag.ConfirmReference = callbackUrl;
                        string textmail = ViewToString("emailConfirmation", null);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", textmail);//"Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    }
                    catch (SmtpException e1)
                    {
                        await UserManager.DeleteAsync(user);
                        ViewBag.UnsuccessfulError = e1.Message;
                        ModelState.AddModelError("", "Не удалось создать пользователя. " + e1.Message);
                        badModel =
                            new Tuple<RegisterVm, LoginVm>(Model, new LoginVm());
                        return View("Welcome", badModel);
                    }
                    
                    return View("RegisterConfirmationSent");
                }
                else
                {
                    ModelState.AddModelError("", "Не удалось создать пользователя. " + result.Errors.First());
                }
            }
            
            // If we got this far, something failed, redisplay form
            badModel = new Tuple<RegisterVm, LoginVm>(Model, new LoginVm());
            return View("Welcome", badModel);
        }

        //
        // GET: /Init/ConfirmEmail
        [AllowAnonymous]
        public ActionResult ConfirmEmailAndChangePassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            ChangePasswordAndConfirmEmailVm model = new ChangePasswordAndConfirmEmailVm { userId = userId, code = code };
            return View("ConfirmEmailAndChangePassword",model);
        }

        //
        // POST: /Init/ConfirmEmail
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmailAndChangePassword( ChangePasswordAndConfirmEmailVm Model )
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                result = await UserManager.ConfirmEmailAsync(HttpUtility.UrlDecode(Model.userId), HttpUtility.UrlDecode(Model.code));
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Произошла ошибка при подтверждении Email. " + result.Errors.First());
                    return View(Model);
                }
                result = await UserManager.RemovePasswordAsync(Model.userId);
                if (! result.Succeeded)
                {
                    ModelState.AddModelError("", "Произошла ошибка при временного пароля. "+result.Errors.First());
                    return View(Model);
                } 
                result = await UserManager.AddPasswordAsync(Model.userId, Model.confirmNewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении нового пароля. " + result.Errors.First());
                    return View(Model);
                }

                return RedirectToAction("Welcome", new { area=""});

            }
            return View(Model);
        }

        // POST: Init/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login( LoginVm Model, string ReturnUrl) 
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = await UserManager.FindAsync(Model.EmailLog, Model.Password);
            if (user != null)
            {
                if (user.EmailConfirmed == true)
                {
                    var result = await SignInManager.PasswordSignInAsync(Model.EmailLog, Model.Password, Model.RememberMe, shouldLockout: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(ReturnUrl);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                           // return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            //                            return View("Welcome", badModel);
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Account is not confirmed. Учётная запись не подтверждена");
                }
            }
            else
            { 
                ModelState.AddModelError("", "Account not found. Учётная запись не найдена");
            }
            Tuple<RegisterVm, LoginVm> badModel =
                     new Tuple<RegisterVm, LoginVm>(new RegisterVm(), Model);
            return View("Welcome", badModel);
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Band", new { area = "" });
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgottenPasswordVm Model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(Model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Init", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
              //  await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Init", new  { area="", callbackUrl = callbackUrl });
            }
            else
            {
                // If we got this far, something failed, redisplay form
                return View(Model);
            }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string callbackUrl = null)
        {
            ViewBag.callbackUrl = callbackUrl;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            ChangePasswordAndConfirmEmailVm model = new ChangePasswordAndConfirmEmailVm { userId = userId, code = code };
            return code == null ? View("Error") : View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ChangePasswordAndConfirmEmailVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(model.userId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account", new { area = "" });
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.code, model.newPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account", new { area = "" });
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        private string ViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Band", new { area = "" });
        }

    }
}
