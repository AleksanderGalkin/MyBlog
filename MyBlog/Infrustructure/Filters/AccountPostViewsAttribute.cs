using log4net;
using MyBlog.Controllers;
using MyBlog.Models;
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrustructure.Filters
{
    public class AccountPostViewsAttribute : FilterAttribute, IActionFilter
    {
        static protected readonly ILog _logger = LogManager.GetLogger("Logger");


        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IDictionary<int, string> post_view_info = filterContext.HttpContext.Session["host"] as IDictionary<int, string>;
            if (post_view_info == null)
            {
                post_view_info = new Dictionary<int, string>();
            }


            int? PostId = null;
            foreach (var parameter in filterContext.ActionParameters)
            {
                if (parameter.Key == "Model")
                {
                    IDeGroupBand model = parameter.Value as IDeGroupBand;
                    if (model != null)
                    {
                        PostId = model.PostId;
                        break;
                    }
                }

            }
            if (PostId == null)
                return;

            if (! post_view_info.ContainsKey((int)PostId))
            {
                string user_host = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
                user_host = user_host == "::1" ? "127.000.000.001" : user_host;
                post_view_info.Add((int)PostId, user_host);
                filterContext.HttpContext.Session["host"] = post_view_info;
                AbstractController controller = filterContext.Controller as AbstractController;
                IUnitOfWork uow = controller._unitOfWork;

               var edited_post = (from p in uow.db.Posts
                                   where p.PostId == PostId
                                   select p)
                                  .SingleOrDefault();
                PostView newPostView = new PostView();



                user_host = user_host.Trim();
                string[] user_host_split = user_host.Split('.');
                for( var i = 0; i < user_host_split.Length; i++)
                {
                    user_host_split[i] = user_host_split[i].PadLeft(3, '0');
                }

                user_host = String.Join(".", user_host_split);

                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string message = createRecord("VAR", controllerName, actionName, "IP" + user_host + "len = " + user_host.Length);
                _logger.Debug(message);

                newPostView.Ip = user_host; ;



                newPostView.Date = DateTime.Now;
                edited_post.PostViews.Add(newPostView);

            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        protected virtual string createRecord(string head, string className, string methodName, string args, string body = "")
        {

            string result = string.Format(@"{0,15}:{1:yyyy\/MM\/dd hh:mm:ss K}-{2}:{3}-User:{4} -Args: {5} - {6}",
                head,
                DateTime.UtcNow,
                className,
                methodName,
                //HttpContext.User.Identity.Name,
                HttpContext.Current.User.Identity.Name,
                args,
                body);
            return result;
        }

    }
}