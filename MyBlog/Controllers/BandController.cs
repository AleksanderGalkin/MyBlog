using AutoMapper;
using Castle.Core;
using Microsoft.AspNet.Identity;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                                     .Select(p=> new PostDisp(p))
                                     .Select(r=>r.GetPostDispVm())
                                     .ToList();
                                     
                                     

            ViewBag.isAuthor = this.isAuthor();
            return View("Index",model);
        }

        //public ActionResult EditPost(int PostId)
        //{
        //    Post Model = (from a in _unitOfWork.db.Posts
        //                  where a.PostId == PostId
        //                  select a)
        //                  .SingleOrDefault();

        //    PostTextEditVm ModelVm = new PostTextEditVm();
        //    ModelVm.Post = Model;
        //    UnicodeEncoding encoding = new UnicodeEncoding();
        //    ModelVm.PostContent = Model.PostContents.FirstOrDefault().ContentData != null ? encoding.GetString(Model.PostContents.FirstOrDefault().ContentData) : "";
        //    ModelVm.Comment = Model.PostContents.FirstOrDefault().Comment ?? "";
        //    return View("EditPost", ModelVm);
        //}

        //[HttpPost]
        //public ActionResult EditPost(PostTextEditVm Model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Post PostModel = (from a in _unitOfWork.db.Posts
        //                          where a.PostId == Model.Post.PostId
        //                          select a)
        //                          .SingleOrDefault();
        //        PostModel.Tittle = Model.Post.Tittle;
        //        PostContent PostContent = PostModel.PostContents.FirstOrDefault();
        //        if (PostContent == null)
        //            PostContent = new PostContent();
        //        PostContent.Comment = Model.Comment ?? "";
        //        UnicodeEncoding encoding = new UnicodeEncoding();
        //        PostContent.ContentData = Model.PostContent != null ? encoding.GetBytes(Model.PostContent) : encoding.GetBytes("");
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View("EditPost",Model);
        //    }

        //}

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

        //[ChildActionOnly]
        //[HttpPost]
        //public virtual ActionResult AuthorControlCreate(PostTextEditVm Model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Post Post = Model.Post;
        //        PostContent PostContent = new PostContent();
        //        UnicodeEncoding encoding = new UnicodeEncoding();
        //        PostContent.ContentData = encoding.GetBytes( Model.PostContent);
        //        PostContent.Comment = Model.Comment;
        //        PostContent.ContentDataType = ContentDataTypes.Text;
        //        Post.PostContents = new Collection<PostContent> { PostContent };
        //        Post.PubDate = DateTime.Now;
        //        Post.ApplicationUserId = User.Identity.GetUserId();
        //        _unitOfWork.db.Posts.Add(Post);

        //    }

        //    if (isAuthor())
        //    {
        //        return View("AuthorControlCreate");
        //    }
        //    else
        //        return null;
        //}

        public ActionResult CreateTextPost()
        {
            PostEdit PostService = new PostEdit(User.Identity.GetUserId(),ContentDataTypes.Text);

            return View("CreateTextPost", PostService.GetPostEditVm<ContentTextEditVm>());
        }

        [HttpPost]
        public ActionResult CreateTextPost(PostEditVm<ContentTextEditVm> Model)
        {
            if (ModelState.IsValid)
            {
                Post Post = Model.Post;
                PostContent PostContent = new PostContent();
                UnicodeEncoding encoding = new UnicodeEncoding();
                PostContent.ContentData = encoding.GetBytes(Model.PostContents.FirstOrDefault().ContentData);
                PostContent.Comment = Model.PostContents.FirstOrDefault() .Comment;
                PostContent.ContentDataType = ContentDataTypes.Text;
                Post.PostContents = new Collection<PostContent> { PostContent };
                Post.PubDate = DateTime.Now;
                Post.ApplicationUserId = User.Identity.GetUserId();
                _unitOfWork.db.Posts.Add(Post);
                return RedirectToAction("Index");
            }
            else
            {
                return View("CreateTextPost", Model);
            }
        }

        //public ActionResult DeletePost(int PostId)
        //{
        //    Post Model = (from a in _unitOfWork.db.Posts
        //                  where a.PostId == PostId
        //                  select a)
        //                              .SingleOrDefault();

        //    PostVm ModelVm = new PostVm();
        //    ModelVm.Post = Model;
        //    ModelVm.PostContents = Model.PostContents;
        //    return View("DeletePost", ModelVm);
        //}

        //[HttpPost]
        //public ActionResult DeletePost(PostTextEditVm Model)
        //{
        //        if (ModelState.IsValid)
        //        {
        //        (from a in _unitOfWork.db.Posts
        //        where a.PostId == Model.Post.PostId
        //        select a)
        //        .ToList()
        //        .ForEach(a => _unitOfWork.db.Posts.Remove(a));

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View("EditPost", Model);
        //    }

        //}

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


        //[ChildActionOnly]
        //public ActionResult GetContentView(PostContent Model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        PostTextDispVm VModel = Mapper.Map<PostContent, PostTextDispVm>(Model);
        //        UnicodeEncoding encoding = new UnicodeEncoding();
        //        VModel.PostContent = Model.ContentData != null ? encoding.GetString(Model.ContentData) : "";
        //        return View("GetContentView", VModel);
        //    }
        //    else
        //        return null;
        //}

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