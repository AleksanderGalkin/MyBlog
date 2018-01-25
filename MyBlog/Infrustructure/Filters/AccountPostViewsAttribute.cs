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
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var host = filterContext.HttpContext.Session["host"] as string;
            if (host == null)
            {
                string SessionId = filterContext.HttpContext.Session.SessionID;
                string user_host = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
                user_host = user_host == "::1" ? "127.000.000.001" : user_host;
                filterContext.HttpContext.Session["host"] = user_host;
                AbstractController controller = filterContext.Controller as AbstractController;
                IUnitOfWork uow = controller._unitOfWork;

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

                var edited_post = (from p in uow.db.Posts
                                   where p.PostId == PostId
                                   select p)
                                  .SingleOrDefault();
                PostView newPostView = new PostView();
                newPostView.Ip = user_host;
                newPostView.Date = DateTime.Now;
                edited_post.PostViews.Add(newPostView);

            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}