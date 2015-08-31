using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using PostSharp.Extensibility;
using PostSharp;

namespace MyBlog.CrossConserns.Exceptions
{
    [Serializable]
    public class ExceptionCatcherAttribute: OnExceptionAspect
    {
        private string _methodName;
        private string _className;

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _methodName = method.Name;
            _className = method.DeclaringType.Name;
        }
        public override bool CompileTimeValidate(MethodBase method)
        {
           return true;

        }
        public override void OnException(MethodExecutionArgs args)
        {
            System.Diagnostics.Debug.WriteLine("SomeText");
            args.FlowBehavior = FlowBehavior.RethrowException;
        }
    }
}