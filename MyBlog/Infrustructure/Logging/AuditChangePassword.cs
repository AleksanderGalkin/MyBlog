using log4net;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using PostSharp.Extensibility;
using PostSharp;

namespace MyBlog.Infrustructure.Logging
{   
    [Serializable]
    public class AuditChangePasswordAttribute : OnMethodBoundaryAspect
    {
        [NonSerialized]
        static private readonly ILog _audit1 = LogManager.GetLogger("Auditor");
        private string _methodName;
        private string _classdName;
        private int _paramIdx=-1;
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _classdName = method.DeclaringType.Name;
            _methodName = method.Name;
            var parameters = method.GetParameters();
            for (int i=0;i<parameters.Count();i++)
            {
                if (parameters[i].Name=="model")
                {
                    _paramIdx = i;
                }
            }
            base.CompileTimeInitialize(method, aspectInfo);
        }
        public override bool CompileTimeValidate(MethodBase method)
        {
            if (_paramIdx == -1)
            {
                Message.Write(MessageLocation.Unknown,SeverityType.Warning, "999",
                    "AuditChangePassword was unable to find an 'model' parameter in {0}.{1}",_classdName, _methodName);
                return false;
            }
            return true;
        }
        public override void OnSuccess(MethodExecutionArgs args)
        {
            var r = args.ReturnValue;
            var v = args.Method;
            _audit1.InfoFormat("Result: {0}",r);
            base.OnSuccess(args);
        }
        public override void OnYield(MethodExecutionArgs args)
        {
            var e = args.ReturnValue;
            base.OnYield(args);
        }
    }
}