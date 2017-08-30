using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Export(typeof(IController)),
    ExportMetadata("Name", ""),
    ExportMetadata("Version", ""),
    ExportMetadata("ControllerName", "PostView"),
    ExportMetadata("ControllerType", typeof(IController))]

    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PostViewController : AbstractController
    {
        IDataStoreBand _ds;
        [ImportingConstructor]
        public PostViewController
                   (IUnitOfWork UnitOfWork,
                   [Import("PluginTextPostType", typeof(IDataStoreBand))]IDataStoreBand DataStore) 
            : base(UnitOfWork)
        {
            _ds = DataStore;
        }

        public ActionResult ShowPost(IDeGroupBand Model)
        {
            var  host = Session["host"] as string;
            if (host == null)
            {
                host = Request.UserHostName;
                string SessionId = HttpContext.Session.SessionID;
                string user_host = HttpContext.Request.UserHostAddress;
                user_host = user_host == "::1" ? "127.000.000.001" : user_host;
                Session["host"] = user_host;
                var edited_post = (from p in _unitOfWork.db.Posts
                                   where p.PostId == Model.PostId
                                   select p)
                                  .SingleOrDefault();
                PostView newPostView = new PostView();
                newPostView.Ip = user_host;
                newPostView.Date = DateTime.Now;
                edited_post.PostViews.Add(newPostView); 

            }
            var post = (from a in _unitOfWork.db.Posts
                                 where a.PostId == Model.PostId_CmdShowPostView
                                 select a)
                                 .SingleOrDefault();
            PostGroupVm model = new PostService(post).GetPostGroupVm();

            return View("ShowPost", model);
        }
    }
}