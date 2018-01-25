using AutoMapper;
using Microsoft.AspNet.Identity;
using MyBlog.Infrastructure.Services;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Export(typeof(IController)),
       ExportMetadata("Name",""),
        ExportMetadata("Version",""),
        ExportMetadata("ControllerName", "Band"),
        ExportMetadata("ControllerType", typeof(IController))]
        
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BandController : AbstractController
    {
        IDataStoreBand _ds;

        [ImportingConstructor]
        public BandController
            (IUnitOfWork UnitOfWork,
            [Import( typeof(IDataStoreBand))]IDataStoreBand DataStore)
            : base(UnitOfWork)
        {
            _ds = DataStore;
        }

 
        public ActionResult Index([Import( typeof(IDataStore<IDsTagModel>))]
                                          IDataStore<IDsTagModel> TagDataStore
                                  )
        {

            IList<PostGroupVm> model = (from a in _unitOfWork.db.Posts
                                       select a)
                            .ToList()
                            .Select(p => new PostService(p))   //// Переделать на AutoMapper ??
                            .Select(r => r.GetPostGroupVm())
                            .ToList();

            ViewBag.isAuthor = this.isAuthor();

            foreach(var item in  model)
            {
                IList<IDsTag> post_tags = (from t in _unitOfWork.db.Tags
                                           join tp in _unitOfWork.db.PostTags
                                           on t.TagId equals tp.TagId
                                           where tp.PostId == item.PostId
                                           select t
                                           )
                                           .ToList()
                                           .Select(t=>
                                               {
                                                   IDsTag tag = PlugInFactory.GetModelByInterface<IDsTag>();
                                                   tag.TagId = t.TagId;
                                                   tag.TagName = t.TagName;
                                                   return tag;

                                               }
                                           )
                                            .ToList<IDsTag>();

                IDsTagModel tag_data = PlugInFactory.GetModelByInterface<IDsTagModel>();
                tag_data.post_tags = post_tags;
                TagDataStore.SetModelByKey("tags"+item.PostId.ToString(), tag_data);

                item.TagSession = "tags" + item.PostId.ToString();
            }

            return View("Index",  model);

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

            if (User.IsInRole("Admin"))
            {
                isAuthor = true;
            }

            return isAuthor;
        }

        public JsonResult js (List<data> m)
        {
            return Json(new { status = "ok" });
        } 
    }

    public class data
    {
        public string model { get; set; }
    }
}