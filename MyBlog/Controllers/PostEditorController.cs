using AutoMapper;
using Microsoft.AspNet.Identity;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class PostEditorController : AbstractController
    {
        public PostEditorController(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
        // GET: PostEditor


        public ActionResult CreatePost()
        {
            PostService PostService = new PostService(User.Identity.GetUserId());
            PostEditVm model = PostService.GetPostEditVm();
            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents != null)
            {
                model.PostContents = Contents;
            }
            return View("EditPost", model);
        }

        public ActionResult CancelPostEdition()
        {
            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents != null)
            {
                Session["PostContents"] = null;
            }
            return RedirectToAction("Index","Band");
        }

        public ActionResult CreateContentText()
        {
            IContentType TextContent = new ContentTextVm();
            return View("EditContent",TextContent);
        }

        public ActionResult CreateContentFile()
        {
            IContentType model = new ContentImageVm();
            return View("EditContent", model);
        }


        [HttpPost]
        public ActionResult EditPost(PostEditVm Model)
        {
            if (ModelState.IsValid)
            {
                Post Post = null;
                if (Model.PostId == 0)
                {
                    Post = Mapper.Map<PostEditVm, Post>(Model);
                    Post.PubDate = DateTime.Now;
                    Post.ApplicationUserId = User.Identity.GetUserId();
                }
                else
                {
                    Post = (from a in _unitOfWork.db.Posts.Include(p=>p.PostContents)
                                      where a.PostId == Model.PostId
                                      select a)
                                  .SingleOrDefault();
                    if (Post == null)
                        throw new NotImplementedException("Post with such Id not found.");
                }
               
                foreach(var mc in Model.PostContents)
                {
                    PostContent content = (from c in Post.PostContents
                                                  where c.PostContentId == mc.PostContentId
                                                  select c)
                                                 .SingleOrDefault();
                    switch (mc.EditMode)
                    {
                        case ContentModeEnum.Create:
                            PostContent pc = GetPostContent(mc);
                            pc.PostContentId = 0;
                            Post.PostContents.Add(pc);
                            break;
                        case ContentModeEnum.Edit:
                            content = GetPostContent(mc);
                            break;
                        case ContentModeEnum.Delete:
                            Post.PostContents.Remove(content);
                            break;
                    }
                }


                if (Model.PostId == 0)
                {
                    _unitOfWork.db.Posts.Add(Post);
                }
               
                return RedirectToAction("Index", "Band");
            }
            else
            {
                return View("EditPost", Model);
            }
        }

        private PostContent GetPostContent (IContentType Content)
        {
            PostContent PostContent = new PostContent();
            switch (Content.ContentDataType)
            {
                case ContentTypeEnums.Text:
                    PostContent = GetPostContentText(Content as ContentTextVm
                , PostContent);
                    break;
                case ContentTypeEnums.Image:
                    PostContent = GetPostContentImage(Content as ContentImageVm
                , PostContent);
                    break;
                case ContentTypeEnums.Sound:
                    break;
                case ContentTypeEnums.Video:
                    break;
                default:
                    throw new NotImplementedException("Unknown content type: " + Content.ContentDataType);
            }
            return PostContent;
        }

        public ActionResult EditPost(int PostId)
        {
            Post Model = (from a in _unitOfWork.db.Posts
                          where a.PostId == PostId
                          select a)
                          .SingleOrDefault();
            PostService PostService = new PostService(Model);
            PostEditVm retModel = PostService.GetPostEditVm();
            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents == null)
            {
                Contents = new List<IContentType>();
            }
            Contents = retModel.PostContents;
            Session["PostContents"] = Contents;
            //IList<IContentType> Contents2 = Session["PostContents"] as IList<IContentType>;
            //var Contents3 = Session["PostContents"];
            //IList<IContentType> Contents4 = Contents3 as IList<IContentType>;

            return View("EditPost", retModel);
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

                return RedirectToAction("Index", "Band");
            }
            else
            {
                return View("EditPost", Model);
            }

        }

        [HttpPost]
        public void LoadFile(HttpPostedFileBase file, string comment)
        {
            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents == null)
            {
                Contents = new List<IContentType>();
            }
            string arViewResults = "";
          //  foreach (var i in files)
           // {
                IContentType newContent = null;
                switch (file.ContentType)
                {
                    case "image/jpeg":
                        ContentImageVm newContentImage = new ContentImageVm();
                        newContentImage.ContentDataType = ContentTypeEnums.Image;
                        MemoryStream s = new MemoryStream();
                        file.InputStream.CopyTo(s);
                        newContentImage.ContentData = s.ToArray();
                        newContent = newContentImage;
                        break;
                    case "video":
                        break;
                    default:
                        throw new NotImplementedException("Неизвестный тип файла");
                }
                var ids = from c in Contents
                          select c.PostContentId;

                int next_id = ids.Count() == 0 ? -1 : ids.Min() - 1;
                newContent.PostContentId = next_id;
                newContent.Comment = comment;
                Contents.Add(newContent);
                arViewResults = arViewResults + this.RenderPartialViewToString("AttachedContent", newContent); // View("AttachedContent", newContent);
            //}
            Session["PostContents"] = Contents;

            string json_object = JsonConvert. SerializeObject(arViewResults,Formatting.Indented);
            HttpContext.Response.Write(arViewResults);
        }

        public ActionResult EditContent (int PostId, int PostContentId)
        {

            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents == null )
            {
                throw new Exception("Session timeout");
            }

            IContentType content = (from c in Contents
                                    where c.PostContentId == PostContentId
                                    select c)
                                    .SingleOrDefault();
          

            return View("EditContent",content);
        }
        [HttpPost]
        public ActionResult EditContent(IContentType Model)
        {
            IContentType returnedModel = null;
            if (ModelState.IsValid)
            {
                IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
                if (Contents == null)
                {
                    throw new Exception("Session timeout");
                }

                if (Model.PostContentId == 0)
                {
                    var ids = from c in Contents
                              where c.PostContentId < 0
                              select c.PostContentId;

                    int next_id = ids.Count() == 0 ? -1 : ids.Min() - 1;
                    Model.PostContentId = next_id;
                    Model.EditMode = ContentModeEnum.Create;
                    Contents.Add(Model);
                    returnedModel = Model;
                }
                else
                {
                    int index = Contents.IndexOf(Contents.Where(c => c.PostContentId == Model.PostContentId)
                                                          .SingleOrDefault());
                    //if (index < 0)
                    //{
                    //    Model.data_edit_diff_flag = !Model.data_edit_diff_flag;
                    //    Contents.Add(Model);
                    //    returnedModel = Model;
                    //}
                    //else
                    //{
                    if (Model.EditMode == ContentModeEnum.None)
                        Model.EditMode = ContentModeEnum.Edit;
                    Contents[index].UpdateFrom(Model);
                    returnedModel = Contents[index];
                    //}
                }

                Session["PostContents"] = Contents;
            }
            return View("AttachedContent", returnedModel as IContentType);
        }


        [HttpPost]
        public void DeleteContent (int PostId, int PostContentId)
        {
            IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
            if (Contents == null )
            {
                throw new Exception("Session timeout");
            }
            else 
            {
                int index = Contents.IndexOf(Contents.Where(c => c.PostContentId == PostContentId)
                                                           .SingleOrDefault());

                Contents[index].EditMode = ContentModeEnum.Delete;
                Session["PostContents"] = Contents;
             }
            string json_object = JsonConvert.SerializeObject(new {id = PostContentId, result = true }, Formatting.Indented);
            HttpContext.Response.Write(json_object);
        }

        PostContent GetPostContentText(ContentTextVm srcModel, PostContent dstModel)
        {
            PostContent result = Mapper.Map<ContentTextVm, PostContent>(srcModel, dstModel);
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


    public static class ControllerExtensions
    {
        /// <summary>
        /// Renders the specified partial view to a string.
        /// </summary>
        /// <param name="controller">The current controller instance.</param>
        /// <param name="viewName">The name of the partial view.</param>
        /// <param name="model">The model.</param>
        /// <returns>The partial view as a string.</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                // Find the partial view by its name and the current controller context.
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                // Create a view context.
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                // Render the view using the StringWriter object.
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

}