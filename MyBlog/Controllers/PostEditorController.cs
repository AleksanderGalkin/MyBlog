using AutoMapper;
using Microsoft.AspNet.Identity;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Export(typeof(IController)),
   ExportMetadata("Name", ""),
    ExportMetadata("Version", ""),
    ExportMetadata("ControllerName", "PostEditor"),
    ExportMetadata("ControllerType", typeof(IController))]

    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PostEditorController : AbstractController
    {
        IDataStorePostManage _ds;

        [ImportingConstructor]
        public PostEditorController(IUnitOfWork UnitOfWork,
             [Import("PluginTextPostType", typeof(IDataStorePostManage))]IDataStorePostManage DataStore)
            : base(UnitOfWork)
        {
            _ds = DataStore;
        }
        // GET: PostEditor


        public ActionResult CreatePost()
        {
            PostService PostService = new PostService(User.Identity.GetUserId());
            PostEditVm model = PostService.GetPostEditVm();
            IList<IDataStoreRecord> Contents = Session["PostContents"] as IList<IDataStoreRecord>;
            if (Contents != null)
            {
                model.PostContents = Contents;
            }
            ViewBag.EditPostmode = "Новый пост";
            return View("EditPost", model);
        }

        public ActionResult CancelPostEdition()
        {
            IList<IDataStoreRecord> Contents = Session["PostContents"] as IList<IDataStoreRecord>;
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

        public ActionResult EditPost(int PostId)
        {
            Post Model = (from a in _unitOfWork.db.Posts
                          where a.PostId == PostId
                          select a)
                          .SingleOrDefault();
            PostService PostService = new PostService(Model);
            PostEditVm retModel = PostService.GetPostEditVm();
            _ds.Clear();
            foreach (var item in retModel.PostContents)
            {
                _ds.Add(item);
            }
            Session["data_store"] = _ds;
            ViewBag.EditPostmode = "Редактирование поста";
            return View("EditPost", retModel);
        }

        [HttpPost]
        public ActionResult EditPost(PostEditVm Model)
        {
            IDataStorePostManage data_store = Session["data_store"] as IDataStorePostManage;
            if (data_store == null)
            {
                throw new HttpException("Session not exist");
            }
            Post post = null;
            if (Model.PostId == 0)
            {
                post = Mapper.Map<Post>(Model);
                post.PubDate = DateTime.Now;
                post.ApplicationUserId = User.Identity.GetUserId();

            }
            else
            {
                post = _unitOfWork.db.Posts.Where(p => p.PostId == Model.PostId).FirstOrDefault();
                post.Tittle = Model.Tittle;
            }

            
            var content_of_post = data_store.Get().Where(c => c.PostId == Model.PostId);
            foreach ( var i in content_of_post)
            {
                PostContent post_content = new PostContent();
                post_content = Mapper.Map<IDataStoreRecord, PostContent>(i,post_content);
                
                switch (i.Status)
                {
                    case IDataStoreRecordStatus.None:
                        break;
                    case IDataStoreRecordStatus.Created:
                        
                        post.PostContents.Add(post_content);
                        break;
                    case IDataStoreRecordStatus.Modified:
                        
                        PostContent current_content = post.PostContents.Where(c=>c.PostContentId == i.PostContentId)
                                                       .SingleOrDefault();
                        current_content.Comment = i.Comment;
                        current_content.ContentData = i.ContentData;
                        current_content.ContentType = i.DataPluginName;
                        break;
                    case IDataStoreRecordStatus.Deleted:
  
                        PostContent current_content_for_del = post.PostContents.Where(c => c.PostContentId == post_content.PostContentId)
                            .SingleOrDefault();
                        post.PostContents.Remove(current_content_for_del);
                        break;
                    default:
                        break;
                }
            }

            if (Model.PostId == 0)
            {
                _unitOfWork.db.Posts.Add(post);
            }

            return RedirectToAction("Index", "Band");
  
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

        


        public ActionResult DeletePost(int PostId)
        {
            Post Post = (from a in _unitOfWork.db.Posts
                         where a.PostId == PostId
                         select a)
                          .SingleOrDefault();

            PostService PostService = new PostService(Post);
            return View("DeletePost", PostService.GetPostDispVm2());
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