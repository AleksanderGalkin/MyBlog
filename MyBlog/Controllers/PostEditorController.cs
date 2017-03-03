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



        public ActionResult CreatePost(string PostPluginName)
        {
            PostService PostService = new PostService(User.Identity.GetUserId());
            PostVm model = PostService.GetPostVm();
            model.PostPluginName = PostPluginName;
            IList<IDataStoreRecord> Contents = Session["PostContents"] as IList<IDataStoreRecord>;
            if (Contents != null)
            {
                model.PostContents = Contents;
            }
            ViewBag.EditPostmode = "Новый пост";
            return View("EditPost", model);
        }

        


        public ActionResult EditPost(int PostId = 0)
        {
            Post Model = new Post();
            if (PostId != 0) // новый пост
            {
                Model = (from a in _unitOfWork.db.Posts
                         where a.PostId == PostId
                         select a)
                        .SingleOrDefault();
            }
            
                
            PostService PostService = new PostService(Model);
            PostVm retModel = PostService.GetPostVm();         // Переделать на AutoMapper ??  
            //_ds.Clear();
            
            ViewBag.EditPostmode = "Редактирование поста";
            return View("EditPost", retModel);
        }

        [HttpPost]
        public ActionResult EditPost(PostVm Model)  //или добавить в PostService логигу обновления из PostVm
        {
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


            //var content_of_post = _ds.GetAllContents().Where(c => c.PostId == Model.PostId);
            var content_of_post = _ds.GetModPost(Model.PostId);
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
                        current_content.ContentPluginName = i.ContentPluginName;
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
            _ds.Clear();
            
            return RedirectToAction("Index", "Band");
  
        }

        public ActionResult DeletePost(int PostId)
        {
            Post Post = (from a in _unitOfWork.db.Posts
                         where a.PostId == PostId
                         select a)
                          .SingleOrDefault();

            PostService PostService = new PostService(Post);
            return View("DeletePost", PostService.GetPostVm());
        }

        [HttpPost]
        public ActionResult DeletePost(PostVm Model)
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

        public ActionResult CancelPostEdition()
        {
            
            if (_ds.GetAllContents().Count() != 0)
            {
                _ds.Clear();
            }
            return RedirectToAction("Index", "Band");
        }


        //    [HttpPost] //LoadFile
        //    public void LoadFile(HttpPostedFileBase file, string comment)
        //    {
        //        IList<IContentType> Contents = Session["PostContents"] as IList<IContentType>;
        //        if (Contents == null)
        //        {
        //            Contents = new List<IContentType>();
        //        }
        //        string arViewResults = "";

        //        IContentType newContent = null;
        //        switch (file.ContentType)
        //        {
        //            case "image/jpeg":
        //                ContentImageVm newContentImage = new ContentImageVm();
        //                newContentImage.ContentDataType = ContentTypeEnums.Image;
        //                MemoryStream s = new MemoryStream();
        //                  file.InputStream.Position = 0;
        //                file.InputStream.CopyTo(s);
        //                newContentImage.ContentData = s.ToArray();
        //                newContent = newContentImage;
        //                break;
        //            case "video":
        //                break;
        //            default:
        //                throw new NotImplementedException("Неизвестный тип файла");
        //        }
        //        var ids = from c in Contents
        //                    select c.PostContentId;

        //        int next_id = ids.Count() == 0 ? -1 : ids.Min() - 1;
        //        newContent.PostContentId = next_id;
        //        newContent.Comment = comment;
        //        Contents.Add(newContent);
        //        arViewResults = arViewResults + this.RenderPartialViewToString("AttachedContent", newContent); // View("AttachedContent", newContent);

        //        Session["PostContents"] = Contents;

        //        string json_object = JsonConvert. SerializeObject(arViewResults,Formatting.Indented);
        //        HttpContext.Response.Write(arViewResults);
        //    }

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