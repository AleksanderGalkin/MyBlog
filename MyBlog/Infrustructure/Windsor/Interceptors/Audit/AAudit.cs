﻿using Castle.DynamicProxy;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Not used now. Mef is used
namespace MyBlog.Infrustructure.Windsor.Interceptors.Audit
{
    public abstract class AAudit : IInterceptor
    {
        static protected readonly ILog _audit1 = LogManager.GetLogger("Auditor");
        public virtual void Intercept(IInvocation invocation)
        {
            if (isApplicable(invocation))
            {
                switch (invocation.Method.Name)
                {
                    case "OnActionExecuting":
                        OnActionExecuting(invocation);
                        break;

                    case "OnActionExecuted":
                        OnActionExecuted(invocation);
                        break;
                }
            }
            invocation.Proceed();
        }

        protected virtual  void OnActionExecuting(IInvocation invocation)
        {
        }
        protected virtual void OnActionExecuted(IInvocation invocation)
        {
        }
        protected abstract bool isApplicable(IInvocation invocation);

        protected virtual string createRecord(string head, string className, string methodName, string args, string body = "")
        {

            string result = string.Format(@"{0,15}:{1:yyyy\/MM\/dd hh:mm:ss K}-{2}:{3}-User:{4} -Args: {5} - {6}",
                head,
                DateTime.UtcNow,
                className,
                methodName,
                HttpContext.Current.User.Identity.Name,
                args,
                body);
            return result;
        }
    }
}