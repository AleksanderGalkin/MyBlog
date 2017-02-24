using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.ViewModels;
using MyBlogContract.Band;
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
            [Import("PluginTextPostType", typeof(IDataStoreBand))]IDataStoreBand DataStore) 
            : base(UnitOfWork)
        {
            _ds = DataStore;
        }

 
        public ActionResult Index()
        {

            IList<PostVm> model = (from a in _unitOfWork.db.Posts
                                       select a)
                            .ToList()
                            .Select(p => new PostService(p))   //// Переделать на AutoMapper ??
                            .Select(r => r.GetPostVm())
                            .ToList();

            ViewBag.isAuthor = this.isAuthor();
            return View("Index",  model);

        }

        public ActionResult ShowContent (int PostId, int ContentId)
        {
            return View();
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