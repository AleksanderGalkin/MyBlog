using MyBlog.Infrastructure.Services;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Filters;
using MyBlog.Infrustructure.Services;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.SessionEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Export(typeof(IController)),
    ExportMetadata("Name", ""),
    ExportMetadata("Version", ""),
    ExportMetadata("ControllerName", "PostView"),
    ExportMetadata("ControllerType", typeof(IController))]

    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PostViewController : AbstractController
    {
        IDataStoreBand _ds;
        [ImportingConstructor]
        public PostViewController
                   (IUnitOfWork UnitOfWork,
                   [Import("PluginTextPostType", typeof(IDataStoreBand))]IDataStoreBand DataStore) 
            : base(UnitOfWork)
        {
            _ds = DataStore;
        }
        [AccountPostViews]
        public ActionResult ShowPost([Import( typeof(IDataStore<IDsTagModel>))]
                                          IDataStore<IDsTagModel> TagDataStore, 
                                    IDeGroupBand Model)
        {
           
            var post = (from a in _unitOfWork.db.Posts
                                 where a.PostId == Model.PostId_CmdShowPostView
                                 select a)
                                 .SingleOrDefault();
            PostGroupVm model = new PostService(post).GetPostGroupVm();

            IList<IDsTag> post_tags = (from t in _unitOfWork.db.Tags
                                       join tp in _unitOfWork.db.PostTags
                                       on t.TagId equals tp.TagId
                                       where tp.PostId == Model.PostId
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
            tag_data.post_tags = post_tags;

            TagDataStore.SetModelByKey("tags", tag_data);

            model.TagSession = "tags";


            return View("ShowPost", model);
        }
    }
}