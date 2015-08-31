using log4net;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure.Logging
{
    [Serializable]
    public class LogAttribute: OnMethodBoundaryAspect
    {
        [NonSerialized]
        static private readonly ILog logger = LogManager.GetLogger("Logger");
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (!args.Method.IsSpecialName)
            {
                logger.Debug(createRecord(args, "Enter Method"));
                base.OnEntry(args);
            }
        }
        public override void OnExit(MethodExecutionArgs args)
        {
            if (!args.Method.IsSpecialName)
            {
                logger.Debug(createRecord(args, "Exit Method"));
                base.OnExit(args);
            }
        }
        public override void OnException(MethodExecutionArgs args)
        {
            string exMessage = args.Exception.Message+ " ";
            if (args.Exception.InnerException != null)
            {
                exMessage = args.Exception.InnerException.Message;
            }
            else
            {
                exMessage = exMessage + "Inner is Null";
            }
            logger.Fatal(createRecord(args, "Exception", exMessage));
            base.OnException(args);
        }


        private string createRecord(MethodExecutionArgs args, string head, string body = "")
        {
           // var typeNames = args.Method.GetGenericArguments().Select(t => t.Name);

            string result = string.Format(@"{0,15}:{1:yyyy\/MM\/dd hh:mm:ss K}-{2}:{3}-User:{4} - {5} - {6}",
                head,
                DateTime.UtcNow,
                args.Method.DeclaringType.Name,
                args.Method.Name,
                HttpContext.Current.User.Identity.Name,
                args.Arguments.ToString(),
                body);
            return result;
        }
    }
}