using AutoMapper;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace MyBlog.Infrustructure.Services
{
    public class PostDisp
    {
        Post _post;
        
        public PostDisp (Post Post)
        {
            _post = Post;
        }
        
        public  PostDispVm GetPostDispVm()
        {
            PostDispVm result = new PostDispVm();
            result.Post = this._post;
            result.PostContents = new Collection<IContentTypeDispVm>();
            foreach (var i in _post.PostContents)
            {
                
                if (i.ContentDataType == ContentDataTypes.Text)
                {
                    ContentTextDispVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentTextDispVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData);
                    result.PostContents.Add(newItem);
                }

                if (i.ContentDataType == ContentDataTypes.Image)
                {
                    ContentImageDispVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentImageDispVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData);
                    result.PostContents.Add(newItem);
                }

            }
            
            return result;
        }

    }

    public class PostEdit
    {
        Post _post;

        public PostEdit(Post Post)
        {
            _post = Post;
        }

        public PostEdit(string parUserId, ContentDataTypes parContentDataType)
        {
            _post = new Post();
            _post.ApplicationUserId = parUserId;
            _post.Tittle = "";
            _post.PostComments = new Collection<PostComment>();
            _post.PostViews = new Collection<PostView>();
            _post.PostTags = new Collection<PostTag>();
            _post.PostContents = new Collection<PostContent>();
            _post.PostContents.Add(new PostContent() { ContentDataType = parContentDataType, ContentData = new byte[0] });
        }

        public PostEditVm<T> GetPostEditVm<T>()
            where T :  class,IContentTypeEditVm
        {
            PostEditVm<T> result = new PostEditVm<T>();
            result.Post = this._post;
            result.PostContents = new Collection<T>();
            foreach (var i in _post.PostContents)
            {

                if (i.ContentDataType == ContentDataTypes.Text)
                {
                    ContentTextEditVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentTextEditVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData);
                    result.PostContents.Add(newItem as T);
                }

                if (i.ContentDataType == ContentDataTypes.Image)
                {
                    ContentImageEditVm newItem = null;
                    newItem = Mapper.Map<PostContent, ContentImageEditVm>(i);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    newItem.ContentData = encoding.GetString(i.ContentData);
                    result.PostContents.Add(newItem as T);
                }

            }

            return result;
        }

    }

}