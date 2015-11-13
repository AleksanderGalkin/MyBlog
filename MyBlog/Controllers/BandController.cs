using Castle.Core;
using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{

    public class BandController : AbstractController
    {

        public BandController(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {

        }

        // GET: Band
        public ActionResult Index()
        {

            bool isAuthor = this.isAuthor();
            IList<PostVm> model = (from a in _unitOfWork.db.Posts
                                        select new PostVm
                                        {
                                            Post = a,
                                            PostContents = a.PostContents,
                                            PostTags = a.PostTags,
                                            isAuthor = isAuthor
                                        }).ToList();

            ViewBag.isAuthor = isAuthor;
            return View("Index",model);
        }

        [ChildActionOnly]
        public ActionResult AuthorControlCreate()
        {
            ViewBag.isAuthor = isAuthor();
            return View();
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult AuthorControlCreate(PostVm Model)
        {
            ViewBag.isAuthor = isAuthor();
            return View();
        }

        

        private bool isAuthor()
        {
            ClaimsIdentity userIdentity = User.Identity as ClaimsIdentity;
            bool isAuthor = false;
            if (userIdentity != null)
            {
                var author = userIdentity.Claims.Where(x => x.Type == "Author").Select(x => x.Value).FirstOrDefault();
                if (author == "true")
                {
                    isAuthor = true;
                }
            }
            return isAuthor;
        }
    }
}