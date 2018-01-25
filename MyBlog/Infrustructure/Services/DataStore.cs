
using AutoMapper;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Services;
using MyBlog.Infrustructure.Sevices;
using MyBlog.Models;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.ContentGroup;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MyBlog.Infrastructure.Services
{
    [Export("PluginTextPostType", typeof(IDataStoreBand))]
    [Export("PluginTextPostType", typeof(IDataStorePostManage))]
    //[InheritedExport]
    //[Export("PluginTextPostType", typeof(IDataStoreFullContent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public  class DataStore : IDataStoreBand, IDataStorePostManage, IDataStoreFullContent, IDataStoreContentGroup
    {
        private IList<IDataStoreRecord> store;
        private IUnitOfWork _unitOfWork;

        [ImportingConstructor]
        public DataStore(IUnitOfWork UnitOfWork)
        {
            store = new List<IDataStoreRecord>();
            _unitOfWork = UnitOfWork;
        }

        public  IEnumerable<IDataStoreRecord> GetAllContents()
        {
            IList<IDataStoreRecord> result = new List<IDataStoreRecord>();
            var t =  (from a in _unitOfWork.db.Posts
                                select a)
                            .ToList()
                            .Select(p => new PostService(p))   //// Переделать на AutoMapper ??
                            .Select(r => r.GetPostVm())
                            .ToList();
            foreach (var post in t)
            {
                foreach (var content in post.PostContents)
                {
                    result.Add(content);
                }

            }
            return result;
        }

        public IDataStoreRecord GetNew()
        {

            IDataStoreRecord Model = PlugInFactory.GetModelByInterface<IDataStoreRecord>( "");
            return Model;
        }

        public IDataStoreRecord GetContent(int Id, int tempPostContentId=0)
        {
            IDataStoreRecord result = null;
            if (tempPostContentId != 0)
            {
                result = (IDataStoreRecord)store
                    .Where(x => x.tempPostContentId == tempPostContentId)
                    .SingleOrDefault();
            }
            else 
            if (Id != 0)
            {
                var post_content = (from a in _unitOfWork.db.PostContents
                                    where a.PostContentId == Id
                                    select a).SingleOrDefault();
                result = GetNew();
                result = Mapper.Map<PostContent, IDataStoreRecord>(post_content,result);
            }
            return result;
        }

        public void Modify(IDataStoreRecord Model)
        {
            if (Model.tempPostContentId != 0 || Model.PostContentId !=0) //modification
            {
                if (Model.PostContentId != 0)
                {
                    Model.Status = IDataStoreRecordStatus.Modified;
                }
                else
                {
                    Model.Status = IDataStoreRecordStatus.Created;
                }
            }   
            else // creation
            {
                Model.Status = IDataStoreRecordStatus.Created;
                if (store.Where(x => x.PostId == Model.PostId).Count() == 0)
                {
                    Model.tempPostContentId =  1;
                }
                else
                {
                    int max_tempPostContentId = store
                                            .Where(x => x.PostId == Model.PostId)
                                            .Max(m => m.tempPostContentId);
                    Model.tempPostContentId = max_tempPostContentId + 1;
                }

                if (Model.Order == 0)
                {
                    var actual_contents = GetActualPost(Model.PostId);
                    if (actual_contents.Count() == 0)
                    {
                        Model.Order = 1;
                    }
                    else
                    {
                        int max_order = actual_contents
                                                .Max(m => m.Order);
                        Model.Order = max_order + 1;
                    }
                }

            }
                

            int idx = store
                .Where(x => x.tempPostContentId == Model.tempPostContentId && x.PostContentId == Model.PostContentId)
                .Select(i => store.IndexOf(i))
                .SingleOrDefault();
            if (idx != 0) store.RemoveAt(idx);
            store.Add(Model);
            
        }

        void IDataStorePostManage.Delete(int PostContentId, int tempPostContentId)
        {
            IDataStoreRecord record = GetContent(PostContentId, tempPostContentId);
            record.Status = IDataStoreRecordStatus.Deleted;
            int idx = store
                  .Where(x => x.tempPostContentId == record.tempPostContentId && x.PostContentId == record.PostContentId)
                  .Select(i => store.IndexOf(i))
                  .SingleOrDefault();

            if (idx != 0) store.RemoveAt(idx);
                store.Add(record);
        }

        public void Clear()
        {
            store.Clear();
        }

        public IEnumerable<IDataStoreRecord> GetDbPost(int PostId)
        {
            IList<IDataStoreRecord> result = new List<IDataStoreRecord>();
            var t = (from a in _unitOfWork.db.Posts
                     select a)
                            .ToList()
                            .Select(p => new PostService(p))   //// Переделать на AutoMapper ??
                            .Select(r => r.GetPostVm())
                            .Where(r => (r.PostId == PostId))
                            .ToList();
            foreach (var post in t)
            {
                foreach (var content in post.PostContents)
                {
                    result.Add(content);
                }

            }
            return result;
        }

        public IEnumerable<IDataStoreRecord> GetModPost(int PostId)
        {

            IList<IDataStoreRecord> result = (from a in store
                     where a.PostId == PostId
                     select a)
                    .ToList();
                            
           
            return result;
        }

        public IEnumerable<IDataStoreRecord> GetActualPost(int PostId)
        {
            IEnumerable<IDataStoreRecord> resultStore = GetModPost(PostId);

            IEnumerable<IDataStoreRecord> resultDb = GetDbPost(PostId);


            IEnumerable<IDataStoreRecord> resultDbMinusStore = from d in resultDb
                                                               where ! resultStore.Any(s=>s.PostContentId == d.PostContentId) 
                                                               select d;
            IEnumerable<IDataStoreRecord> result = resultDbMinusStore.Union(resultStore);

            return result;
        }

        public IEnumerable<IDataStoreRecord> GetGroupContent(int PostId, int Order)
        {
            IEnumerable<IDataStoreRecord> result = GetActualPost(PostId).Where (x=>x.Order == Order);
            return result;
        }

    }
}