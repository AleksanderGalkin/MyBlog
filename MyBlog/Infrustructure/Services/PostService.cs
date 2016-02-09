using AutoMapper;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

        public PostService(string parUserId, ContentTypeEnums parContentDataType)
        {
            _post = new Post();
            _post.ApplicationUserId = parUserId;
            _post.Tittle = "";
            _post.PostComments = new Collection<PostComment>();
            _post.PostViews = new Collection<PostView>();
            _post.PostTags = new Collection<PostTag>();
            _post.PostContents = new Collection<PostContent>();
           // _post.PostContents.Add(new PostContent() { ContentDataType = parContentDataType, ContentData = new byte[0] });
        }

        public PostDispVm GetPostDispVm()
        {
            PostDispVm result = new PostDispVm();

            

            result = Mapper.Map<Post, PostDispVm>(_post);
            result.PostContents = new List<IContentType>();
            
            foreach (var i in _post.PostContents)
            {
                
                if (i.ContentDataType == ContentTypeEnums.Text)
                {
                    ContentTextVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentTextVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData ?? encoding.GetBytes(""));
                    result.PostContents.Add(newItem);
                }

                if (i.ContentDataType == ContentTypeEnums.Image)
                {
                    ContentImageVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentImageVm>(i);
                    MemoryStream ms = new MemoryStream(i.ContentData );
                    newItem.ContentData = i.ContentData;
                    result.PostContents.Add(newItem);
                }

            }
            
            return result;
        }

        public PostEditVm GetPostEditVm()
        {
            PostEditVm result = new PostEditVm();
            result = Mapper.Map<Post, PostEditVm>(_post);
            result.PostContents = new List<IContentType>();

            foreach (var i in _post.PostContents)
            {

                if (i.ContentDataType == ContentTypeEnums.Text)
                {
                    ContentTextVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentTextVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData);
                    result.PostContents.Add(newItem);
                }

                if (i.ContentDataType == ContentTypeEnums.Image)
                {
                    ContentImageVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentImageVm>(i);
                    try
                    {
                        newItem.ContentData = i.ContentData;
                    }
                    catch (ArgumentException ex)
                    {
                        newItem.ContentData = null;
                    }
                    result.PostContents.Add(newItem);
                }

            }

            return result;
        }


    }

    //public class PostEdit
    //{
    //    Post _post;

    //    public PostEdit(Post Post)
    //    {
    //        _post = Post;
    //    }

    //    public PostEdit(string parUserId, ContentTypeEnums parContentDataType)
    //    {
    //        _post = new Post();
    //        _post.ApplicationUserId = parUserId;
    //        _post.Tittle = "";
    //        _post.PostComments = new Collection<PostComment>();
    //        _post.PostViews = new Collection<PostView>();
    //        _post.PostTags = new Collection<PostTag>();
    //        _post.PostContents = new Collection<PostContent>();
    //        _post.PostContents.Add(new PostContent() { ContentDataType = parContentDataType, ContentData = new byte[0] });
    //    }

    //    public PostEditVm<T> GetPostEditVm<T>()
    //        where T :  class, IContentType
    //    {
    //        PostEditVm<T> result = new PostEditVm<T>();
    //        result.PostContents = new Collection<T>();
    //        foreach (var i in _post.PostContents)
    //        {

    //            if (i.ContentDataType == ContentTypeEnums.Text)
    //            {
    //                ContentTextVm newItem = null;
    //                newItem = Mapper.Map<PostContent, ContentTextVm>(i);
    //                UnicodeEncoding encoding = new UnicodeEncoding();
    //                newItem.ContentData = encoding.GetString(i.ContentData);
    //                result.PostContents.Add(newItem as T);

    //            }

    //            if (i.ContentDataType == ContentTypeEnums.Image)
    //            {
    //                ContentImageVm newItem = null;
    //                newItem = Mapper.Map<PostContent, ContentImageVm>(i);
    //                UnicodeEncoding encoding = new UnicodeEncoding();
    //                newItem.ContentData = encoding.GetString(i.ContentData);
    //                result.PostContents.Add(newItem as T);
    //            }

    //        }

    //        return result;
    //    }

    //}

}