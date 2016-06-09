using Castle.DynamicProxy;
using log4net;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Not used now. Mef is used
namespace MyBlog.Infrustructure.Windsor.Interceptors.Audit
{
    public class AuditChangeAccountInfoInterceptor : AAudit
    {
        protected override bool isApplicable(IInvocation invocation)
        {
            if (invocation.TargetType.Name == "ManageController")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        override protected void OnActionExecuting(IInvocation invocation)
        {
            
                ActionExecutingContext arg0_1;
                arg0_1 = invocation.Arguments[0] as ActionExecutingContext;

                if (arg0_1.ActionDescriptor.ActionName == "Edit"
                        && arg0_1.HttpContext.Request.RequestType == "POST")
                {
                    var args = string.Join(",", arg0_1.ActionParameters.Select(x => x.Key + " = " + x.Value));
                    _audit1.Info(createRecord("AuditChangeAccountInfo"
                                    , arg0_1.ActionDescriptor.ControllerDescriptor.ControllerType.BaseType.Name
                                    , arg0_1.ActionDescriptor.ActionName
                                    , args
                                    , "Попытка изменения учётных данных"));
                }
            
        }

        override protected void OnActionExecuted(IInvocation invocation)
        {
                ActionExecutedContext arg0_3;
                arg0_3 = invocation.Arguments[0] as ActionExecutedContext;

                if (arg0_3.ActionDescriptor.ActionName == "Edit"
                        && arg0_3.HttpContext.Request.RequestType == "POST")
                {
                    if (typeof(RedirectToRouteResult) == arg0_3.Result.GetType())
                    {
                        _audit1.Info(createRecord("AuditChangeAccountInfo"
                                       , arg0_3.ActionDescriptor.ControllerDescriptor.ControllerType.BaseType.Name
                                       , arg0_3.ActionDescriptor.ActionName
                                       , ""
                                       , "Попытка изменения учётных данных завершилась успешно"));

                    }
                    else if (typeof(ViewResult) == arg0_3.Result.GetType())
                    {
                        _audit1.Info(createRecord("AuditChangeAccountInfo"
                                       , arg0_3.ActionDescriptor.ControllerDescriptor.ControllerType.BaseType.Name
                                       , arg0_3.ActionDescriptor.ActionName
                                       , ""
                                       , "Попытка изменения учётных данных завершилась не успешно"));
                    }
                }
            
        }
    }
}