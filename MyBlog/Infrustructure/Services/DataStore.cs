﻿
using MyBlog.Infrustructure.Sevices;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MyBlog.Infrastructure.Services
{
    [Export("PluginTextPostType",typeof(IDataStoreBand))]
    [Export("PluginTextPostType", typeof(IDataStorePostManage))]
    [Export("PluginTextPostType", typeof(IDataStoreFullContent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public  class DataStore : IDataStoreBand, IDataStorePostManage, IDataStoreFullContent
    {
        private IList<IDataStoreRecord> store;
        public DataStore()
        {
             store = new List<IDataStoreRecord>();
        }

        public void Add(IDataStoreRecord Record)
        {
            store.Add(Record);
        }

        public  IEnumerable<IDataStoreRecord> Get()
        {
            return store.Select(x=>x);
        }

        public IDataStoreRecord GetNew()
        {
            IDataStoreRecord Model = new DataStoreRecord();
            return Model;
        }

        public IDataStoreRecord Get(int Id)
        {
            IDataStoreRecord Model = (IDataStoreRecord)store.Where(m=>m.PostContentId == Id).FirstOrDefault();
            return Model;
        }

        public IDataStoreRecord Get(int Id, int _temporary_PostContentId)
        {
            IDataStoreRecord result = null;
            if (Id != 0)
            {
                result = (IDataStoreRecord)store
                    .Where(m => m.PostContentId == Id)
                    .FirstOrDefault();
            }
            else
            if (Id == 0 && _temporary_PostContentId != 0)
            {
                result = (IDataStoreRecord)store
                    .Where(x => x._temporary_PostContentId == _temporary_PostContentId)
                    .SingleOrDefault();
            }

            return result;
        }

        public void Modify(IDataStoreRecord Model)
        {
            if(Model.PostContentId != 0)
            {
                Model.Status = IDataStoreRecordStatus.Modified;
                int idx = store
                    .Where(x => x.PostContentId == Model.PostContentId)
                    .Select(i => store.IndexOf(i))
                    .SingleOrDefault();
                store.RemoveAt(idx);
                store.Add(Model);
            }
            else
            if(Model.PostContentId == 0 && Model._temporary_PostContentId != 0)
            {
                Model.Status = Model.Status;
                int idx = store
                    .Where(x => x._temporary_PostContentId == Model._temporary_PostContentId)
                    .Select(i => store.IndexOf(i))
                    .SingleOrDefault();
                store.RemoveAt(idx);
                store.Add(Model);
            }
            
        }

        void IDataStorePostManage.Create(IDataStoreRecord Record)
        {
            Record.Status = IDataStoreRecordStatus.Created;
            store.Add(Record);
        }

        void IDataStorePostManage.Delete(int Id, int _temporary_PostContentId)
        {
            if (Id != 0)
            {
                int idx = store.Where(x => x.PostContentId == Id).Select(i => store.IndexOf(i)).SingleOrDefault();
                store.ElementAt(idx).Status = IDataStoreRecordStatus.Deleted;
            }
            else
            if (Id == 0 && _temporary_PostContentId != 0)
            {
                int idx = store.Where(x => x._temporary_PostContentId == _temporary_PostContentId)
                    .Select(i => store.IndexOf(i))
                    .SingleOrDefault();
                store.ElementAt(idx).Status = IDataStoreRecordStatus.Deleted;
            }
        }

        IDataStoreRecord IDataStorePostManage.Get(int Id, int _temporary_PostContentId)
        {
            IDataStoreRecord result = null;
            if (Id != 0)
            {
                result = (IDataStoreRecord)store
                    .Where(m => m.PostContentId == Id)
                    .FirstOrDefault();
            }
            else
            if (Id == 0 && _temporary_PostContentId != 0)
            {
                result = (IDataStoreRecord)store
                    .Where(x => x._temporary_PostContentId == _temporary_PostContentId)
                    .SingleOrDefault();
            }

            return result;
        }

        public void Clear()
        {
            store.Clear();
        }
    }
}