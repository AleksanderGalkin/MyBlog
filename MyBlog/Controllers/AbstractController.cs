using log4net;
using MyBlog.Infrustructure;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class AbstractController : Controller
    {
       
        internal protected IUnitOfWork _unitOfWork { get; set; }
        static protected readonly ILog _logger = LogManager.GetLogger("Logger");

        public AbstractController (IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _unitOfWork.BeginTransaction();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                _unitOfWork.Commit();
                
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = false;

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            var parameters = filterContext.RouteData.Values;

            string ex = filterContext.Exception.Message +". " + filterContext.Exception.InnerException ?? "" ;

            Type ex_type = filterContext.Exception.GetType();
            if (ex_type == typeof(DbEntityValidationException))
            {
                DbEntityValidationException dbe_ex = filterContext.Exception as DbEntityValidationException;
                foreach (var eve in dbe_ex.EntityValidationErrors)
                {
                    ex = ex + String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        ex = ex + String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            ex = ex + filterContext.Exception.StackTrace;


            string message = createRecord("Ex", controllerName, actionName, "", ex);
            _logger.Debug(message);

            base.OnException(filterContext);
        }

        protected virtual string createRecord(string head, string className, string methodName, string args, string body = "")
        {

            string result = string.Format(@"{0,15}:{1:yyyy\/MM\/dd hh:mm:ss K}-{2}:{3}-User:{4} -Args: {5} - {6}",
                head,
                DateTime.UtcNow,
                className,
                methodName,
                HttpContext.User.Identity.Name,
                //HttpContext.User Current.User.Identity.Name,
                args,
                body);
            return result;
        }
    }

}