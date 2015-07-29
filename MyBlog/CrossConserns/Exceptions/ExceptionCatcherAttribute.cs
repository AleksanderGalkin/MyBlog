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
            Message.Write( MessageLocation.Unknown,SeverityType.Warning, "001", "I am applying ExceptionCatcherAttribute to {0} of class {1}.", method.Name, method.DeclaringType.ToString());
            return true;

        }
        public override void OnException(MethodExecutionArgs args)
        {
            // Console.WriteLine("Exception occured in {0}.{1}", _className, _methodName);
            System.Diagnostics.Debug.WriteLine("SomeText");
            args.FlowBehavior = FlowBehavior.RethrowException;
        }
    }
}