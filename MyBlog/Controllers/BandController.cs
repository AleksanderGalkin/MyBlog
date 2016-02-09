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
                                     .Select(p=> new PostService(p))
                                     .Select(r=>r.GetPostDispVm())
                                     .ToList();
                                     
                                     

            ViewBag.isAuthor = this.isAuthor();
            return View("Index",model);
        }
        
        public ActionResult EditPost(int PostId)
        {
            Post Model = (from a in _unitOfWork.db.Posts
                          where a.PostId == PostId
                          select a)
                          .SingleOrDefault();

            PostService PostService = new PostService(Model);
            return View("EditPost", PostService.GetPostEditVm());
        }

        [HttpPost]
        public ActionResult EditPost(PostEditVm Model)
        {
            if (ModelState.IsValid)
            {
                Post PostModel = (from a in _unitOfWork.db.Posts
                                  where a.PostId == Model.PostId
                                  select a)
                                  .SingleOrDefault();
                if (PostModel == null)
                    throw new NotImplementedException("Post with such Id not found.");
                PostModel.Tittle = Model.Tittle;
                if (PostModel.PostContents == null)
                    PostModel.PostContents = new Collection<PostContent>();
                for (int i = 0; i < PostModel.PostContents.Count; i++)
                {
                    PostContent curPostContent = PostModel.PostContents.ElementAt(i);
                    int PostContentId = curPostContent.PostContentId;
                    IContentType ModelContent = Model.PostContents
                                        .Where(x => x.PostContentId == PostContentId)
                                        .SingleOrDefault() ;
                    switch (ModelContent.ContentDataType)
                    {
                        case ContentTypeEnums.Text:
                            PostModel.PostContents[i] = GetPostContentText(ModelContent as ContentTextVm, curPostContent);
                            break;
                        case ContentTypeEnums.Image:
                            break;
                        case ContentTypeEnums.Sound:
                            break;
                        case ContentTypeEnums.Video:
                            break;
                        default:
                            throw new NotImplementedException("Unknown content type: " + ModelContent.ContentDataType);
                    }


                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("EditPost", Model);
            }

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

        public ActionResult CreateTextPost()
        {
            PostService PostService = new PostService(User.Identity.GetUserId(), ContentTypeEnums.Text);
            PostEditVm model = PostService.GetPostEditVm();
            model.PostContents.Add( new ContentTextVm() { ContentDataType = ContentTypeEnums.Text, ContentData = "" });
            return View("CreateTextPost", model);
        }
        public ActionResult CreatePhotoPost()
        {
            PostService PostService = new PostService(User.Identity.GetUserId(), ContentTypeEnums.Image);
            return View("CreateImagePost", PostService.GetPostEditVm());
        }
        [HttpPost]
        public ActionResult CreatePost(PostEditVm Model)
        {
            if (ModelState.IsValid)
            {
                Post Post = Mapper.Map<PostEditVm,Post>(Model);
                List<PostContent> PostContents = new List<PostContent>();
                UnicodeEncoding encoding = new UnicodeEncoding();

                for (int i = 0; i < Model.PostContents.Count; i++)
                {
                    PostContent PostContent = new PostContent();
                    IContentType ModelContent = Model.PostContents[i];
                    PostContent = new PostContent();
                    switch (ModelContent.ContentDataType)
                    {
                        case ContentTypeEnums.Text:
                            PostContent = GetPostContentText(ModelContent as ContentTextVm
                        ,PostContent);
                            break;
                        case ContentTypeEnums.Image:
                            PostContent = GetPostContentImage(ModelContent as ContentImageVm
                        , PostContent);
                            break;
                        case ContentTypeEnums.Sound:
                            break;
                        case ContentTypeEnums.Video:
                            break;
                        default:
                            throw new NotImplementedException("Unknown content type: " + ModelContent.ContentDataType);
                    }
                    PostContents.Add(PostContent);

                }
                Post.PubDate = DateTime.Now;
                Post.ApplicationUserId = User.Identity.GetUserId();
                Post.PostContents = PostContents;
                _unitOfWork.db.Posts.Add(Post);
                return RedirectToAction("Index");
            }
            else
            {
                return View("CreateTextPost", Model);
            }
        }

        public ActionResult DeletePost(int PostId)
        {
            Post Post = (from a in _unitOfWork.db.Posts
                         where a.PostId == PostId
                         select a)
                          .SingleOrDefault();

            PostService PostService = new PostService(Post);
            return View("DeletePost", PostService.GetPostDispVm());
        }

        [HttpPost]
        public ActionResult DeletePost(PostDispVm Model)
        {
            if (ModelState.IsValid)
            {
                (from a in _unitOfWork.db.Posts
                 where a.PostId == Model.PostId
                 select a)
                .ToList()
                .ForEach(a => _unitOfWork.db.Posts.Remove(a));

                return RedirectToAction("Index");
            }
            else
            {
                return View("EditPost", Model);
            }

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

        [HttpPost]
        public void LoadFile(IEnumerable<HttpPostedFileBase> files)
        {
            ICollection <IContentType> Contents = Session["PostContents"] as Collection<IContentType>;
            if (Contents == null)
            {
                Contents = new Collection<IContentType>();
            }
            foreach (var i in files)
            {
                IContentType newContent = null;
                switch (i.ContentType)
                {
                    case "image/jpeg":
                        ContentImageVm newContentImage = new ContentImageVm();
                        newContentImage.ContentDataType = ContentTypeEnums.Image;
                        MemoryStream s = new MemoryStream();
                        i.InputStream.CopyTo(s);
                        newContentImage.ContentData = s.ToArray();
                        newContent = newContentImage;
                        break;
                    case "video":
                        break;
                    default:
                        throw new NotImplementedException("Неизвестный тип файла");
                }
                Contents.Add(newContent);
            }
            Session["PostContents"] = Contents;
            IList<ViewDataUploadFilesResult> r = new List<ViewDataUploadFilesResult>();
            r.Add(new ViewDataUploadFilesResult
            {   url = "www.url.ru",
                name = "asd",
                type = "jpeg"});
            var UploadedFiles = new
            {files = r.ToArray() };

            string json_object = JsonConvert.SerializeObject(UploadedFiles, Formatting.Indented);
            HttpContext.Response.Write(json_object);
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

        PostContent GetPostContentText(ContentTextVm srcModel, PostContent dstModel)
        {
            PostContent result = Mapper.Map<ContentTextVm, PostContent>(srcModel,dstModel);
            UnicodeEncoding encoding = new UnicodeEncoding();
            result.ContentData = encoding.GetBytes(srcModel.ContentData);
            return result;

        }
        PostContent GetPostContentImage(ContentImageVm srcModel, PostContent dstModel)
        {
            PostContent result = Mapper.Map<ContentImageVm, PostContent>(srcModel, dstModel);
            result.ContentData = srcModel.ContentData;
            return result;

        }
    }
}