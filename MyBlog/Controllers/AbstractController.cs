using MyBlog.Infrustructure;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class AbstractController : Controller
    {
       
        protected IUnitOfWork _unitOfWork { get; set; }
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
    }

}