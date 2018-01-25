using AutoMapper;
using MyBlog.Infrastructure.Services;
using MyBlog.Infrustructure.Sevices;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MyBlog.Infrustructure.Services
{
    public class PostService
    {
        Post _post;
        
        public PostService (Post Post)
        {
            _post = Post;
        }

        public PostService(string parUserId)
        {
            _post = new Post();
            _post.ApplicationUserId = parUserId;
            _post.Tittle = "";
            _post.PostComments = new Collection<PostComment>();
            _post.PostViews = new Collection<PostView>();
            _post.PostTags = new Collection<PostTag>();
            _post.PostContents = new Collection<PostContent>();
        }


        public PostVm GetPostVm()
        {
            PostVm result = new PostVm();
            result = Mapper.Map<Post, PostVm>(_post);
            result.PostContents = new List<IDataStoreRecord>();

            foreach (var i in _post.PostContents)
            {
                // IDataStoreRecord newItem = (IDataStoreRecord)PlugInFactory.GetModelByInterface(typeof(IDataStoreRecord), "");
                IDataStoreRecord newItem = new DataStoreRecord();
                newItem = Mapper.Map<PostContent, IDataStoreRecord>(i,newItem);
                result.PostContents.Add(newItem);
            }

            return result;
        }

        public PostGroupVm GetPostGroupVm()
        {
            PostVm PostVm = new PostVm();
            PostVm = Mapper.Map<Post, PostVm>(_post);
            PostVm.PostContents = new List<IDataStoreRecord>();

            foreach (var i in _post.PostContents)
            {
                // IDataStoreRecord newItem = (IDataStoreRecord)PlugInFactory.GetModelByInterface(typeof(IDataStoreRecord), "");
                IDataStoreRecord newItem = new DataStoreRecord();
                newItem = Mapper.Map<PostContent, IDataStoreRecord>(i, newItem);
                PostVm.PostContents.Add(newItem);
            }

            //PostVm.PostViewCount = _post.PostViews.Count;

            var group = from r in PostVm.PostContents
                        group r by new { PostId = r.PostId,
                            Order = r.Order,
                            PostPluginName = r.ContentPluginName
                        }
                        into g
                        select g;

            IList<GroupVmDisplay> result_group = group.Select(x => new GroupVmDisplay
            {
                PostId = x.Key.PostId,
                Order = x.Key.Order,
                GroupPluginName = x.Key.PostPluginName,
                Contexts = x.ToList()
                  
            })
            .OrderBy(o => o.Order)
            .ToList();
            //PostGroupVm PostGroupVm = new PostGroupVm();
            PostGroupVm PostGroupVm =  Mapper.Map<PostVm,PostGroupVm>(PostVm);
            PostGroupVm.GroupPostContents = new List<GroupVmDisplay>(result_group);

            return PostGroupVm;
        }


 
    }


}