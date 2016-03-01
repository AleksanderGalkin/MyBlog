using AutoMapper;
using Castle.Core;
using Microsoft.AspNet.Identity;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            IList<PostDispVm> model = (from a in _unitOfWork.db.Posts
                                       select a)
                            .ToList()
                            .Select(p => new PostService(p))
                            .Select(r => r.GetPostDispVm())
                            .ToList();
            ViewBag.isAuthor = this.isAuthor();
            return View("Index", model);

        }

        [ChildActionOnly]
        public virtual ActionResult AuthorControlCreate()
        {
            if (isAuthor())
            {
                return View("AuthorControlCreate");
            }
            else
                return null;
        }

        [ChildActionOnly]
        public virtual ActionResult AuthorControlEdit(int PostId)
        {
            if (isAuthor())
            {
                ViewBag.PostId = PostId;
                return View("AuthorControlEdit");
            }
            else
                return null;
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