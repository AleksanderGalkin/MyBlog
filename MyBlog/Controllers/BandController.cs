using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            IList<PostVm> model = (from a in _unitOfWork.db.Posts
                                        select new PostVm
                                        {
                                            Post = a,
                                            PostContents = a.PostContents,
                                            PostTags = a.PostTags
                                        }).ToList();

            
            return View("Index",model);
        }
    }
}