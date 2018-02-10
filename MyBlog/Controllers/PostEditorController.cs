using AutoMapper;
using Microsoft.AspNet.Identity;
using MyBlog.Infrastructure.Services;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.PostManage;
using MyBlogContract.TagManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
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
             [Import("PluginTextPostType", typeof(IDataStorePostManage))]IDataStorePostManage DataStore
            )
            : base(UnitOfWork)
        {
            _ds = DataStore;

        }



        public ActionResult CreatePost(string PostPluginName)
        {
            PostService PostService = new PostService(User.Identity.GetUserId());
            PostVm model = PostService.GetPostVm();
           // model.PostPluginName = PostPluginName;
            IList<IDataStoreRecord> Contents = Session["PostContents"] as IList<IDataStoreRecord>;
            if (Contents != null)
            {
                model.PostContents = Contents;
            }
            ViewBag.EditPostmode = "Новый пост";
            return View("EditPost", model);
        }

        


        public ActionResult EditPost(  [Import( typeof(IDataStore<IDsTagModel>))]
                                          IDataStore<IDsTagModel> TagDataStore
                                       , 
                                         int PostId = 0)
        {
            Post Model = new Post();
            if (PostId != 0) // не новый пост
            {
                Model = (from a in _unitOfWork.db.Posts
                         where a.PostId == PostId
                         select a)
                        .SingleOrDefault();

            }
            
                
            PostService PostService = new PostService(Model);
            PostVm retModel = PostService.GetPostVm();         // Переделать на AutoMapper ??  

            if (TagDataStore != null)
            {
                IList<IDsTag> all_tags = (from t in _unitOfWork.db.Tags
                                          select t
                                        )
                                        .ToList()
                                        .Select(t =>
                                        {
                                            IDsTag tag = PlugInFactory.GetModelByInterface<IDsTag>();
                                            tag.TagId = t.TagId;
                                            tag.TagName = t.TagName;
                                            return tag;

                                        }
                                        )
                                        .ToList<IDsTag>();

                IList<IDsTag> post_tags = (from t in _unitOfWork.db.Tags
                                           join tp in _unitOfWork.db.PostTags
                                           on t.TagId equals tp.TagId
                                           where tp.PostId == PostId
                                           select t
                                            )
                                            .ToList()
                                            .Select(t =>
                                            {
                                                IDsTag tag = PlugInFactory.GetModelByInterface<IDsTag>();
                                                tag.TagId = t.TagId;
                                                tag.TagName = t.TagName;
                                                return tag;

                                            }
                                            )
                                            .ToList<IDsTag>();


                IDsTagModel tag_data = PlugInFactory.GetModelByInterface<IDsTagModel>();
                tag_data.all_tags = all_tags;
                tag_data.post_tags = post_tags;

                TagDataStore.SetModelByKey("tags", tag_data);

                retModel.TagSession = "tags";
            }

            ViewBag.EditPostmode = "Редактирование поста";
            return View("EditPost", retModel);
        }

        [HttpPost]
        public ActionResult EditPost([Import( typeof(IDataStore<IDsTagModel>))]
                                          IDataStore<IDsTagModel> TagDataStore,
                                    PostVm Model)  //или добавить в PostService логигу обновления из PostVm
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

            if (TagDataStore != null)
            {
                var tag_data = TagDataStore.GetModelByKey("tags");
                var tags_to_delete = (from s in post.PostTags
                                      where !tag_data.post_tags.Any(si => si.TagId == s.TagId)
                                      select s)
                        .ToList();

                var tags_to_add = (from db_tag in _unitOfWork.db.Tags.ToList()
                                   join web_tag in tag_data.post_tags
                                   on db_tag.TagId equals web_tag.TagId
                                   where !post.PostTags.Any(si => si.TagId == db_tag.TagId)
                                   select db_tag)
                                .ToList();

                foreach (var i in tags_to_delete)
                {
                    post.PostTags.Remove(i);
                }

                foreach (var i in tags_to_add)
                {
                    PostTag new_post_tag = new PostTag { Post = post, Tag = i };
                    post.PostTags.Add(new_post_tag);
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