using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Infrustructure;
using MyBlogContract.TagManage;

namespace MyBlog.Controllers
{

    [Export(typeof(IController)),
   ExportMetadata("Name", ""),
    ExportMetadata("Version", ""),
    ExportMetadata("ControllerName", "SearchHost"),
    ExportMetadata("ControllerType", typeof(IController))]

    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SearchHostController : AbstractController
    {
        [ImportingConstructor]
        public SearchHostController(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }


        public ActionResult SearchByTags(IDtoTag inDto)
        {
            List<int> pids = _unitOfWork.db.Posts
                            .Join(_unitOfWork.db.PostTags
                                    , p => p.PostId
                                    , pt => pt.PostId
                                     ,(p, pt) =>
                                      new { post_id = p.PostId, tag_id = pt.TagId })
                              .Where(pt => pt.tag_id == inDto.TagId_CmdShowPostByTag)
                              .Select(pt => pt.post_id)
                              .ToList();
            TempData["PostIds"] = pids;

            return RedirectToAction("Index", "Band");
        }
    }
}