using Castle.DynamicProxy;
using log4net;
using MyBlog.Infrustructure.Windsor.Interceptors.Log;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Not used now. Mef is used
namespace MyBlog.Infrustructure.Windsor.Interceptors.Audit
{
    public class LoggerInterceptor : ALog
    {
        protected override bool isApplicable(IInvocation invocation)
        {
            if (invocation.TargetType.IsSubclassOf(typeof(Controller)))
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
                var args = string.Join(",", arg0_1.ActionParameters.Select(x => x.Key + " = " + x.Value));
                _logger.Info(createRecord("Entry"
                                , arg0_1.ActionDescriptor.ControllerDescriptor.ControllerType.BaseType.Name
                                , arg0_1.ActionDescriptor.ActionName
                                , args
                                , ""));
            
        }

        override protected void OnActionExecuted(IInvocation invocation)
        {
                ActionExecutedContext arg0_3;
                arg0_3 = invocation.Arguments[0] as ActionExecutedContext;
                _logger.Info(createRecord("Exit"
                                , arg0_3.ActionDescriptor.ControllerDescriptor.ControllerType.BaseType.Name
                                , arg0_3.ActionDescriptor.ActionName
                                , ""
                                , ""));

            
        }
        protected override void OnResultExecuting(IInvocation invocation)
        {
            ResultExecutingContext arg;
            arg = invocation.Arguments[0] as ResultExecutingContext;
            var args = string.Join(",",arg.RouteData.Values.Select(x=> x.Key+" = "+x.Value));
            _logger.Info(createRecord("ResultEntry"
                            , arg.Controller.GetType().BaseType.Name
                            , arg.Result.ToString()
                            , args
                            , ""));
        }
        protected override void OnResultExecuted(IInvocation invocation)
        {
            ResultExecutedContext arg;
            arg = invocation.Arguments[0] as ResultExecutedContext;
            var args = string.Join(",", arg.RouteData.Values.Select(x => x.Key + " = " + x.Value));
            _logger.Info(createRecord("ResultExit"
                            , arg.Controller.GetType().BaseType.Name
                            , arg.Result.ToString()
                            , args
                            , ""));
        }
        protected override void OnException(IInvocation invocation)
        {
            ExceptionContext arg;
            arg = invocation.Arguments[0] as ExceptionContext;
            var args = string.Join(",", arg.RouteData.Values.Select(x => x.Key + " = " + x.Value));
            string message = arg.Exception.Message;
            if(arg.Exception.InnerException != null)
            {
                message = message +" "+ arg.Exception.InnerException.Message;
            }

            if (arg.Exception.GetType() ==  typeof(DbEntityValidationException))
            {
                DbEntityValidationException dbEx = arg.Exception as DbEntityValidationException;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        message = message + "," + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            message = message + arg.Exception.StackTrace;
            _logger.Fatal(createRecord("Exception"
                            , arg.Controller.GetType().Name
                            , arg.Result.ToString()
                            , args
                            , message));
        }
    }
}