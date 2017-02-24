using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Tests
{
    public class DbContextFillFactory
    {
        private static IDbContext context;
        public static IDbContext CurrentContext
        {
            get
            {
                if (context != null)
                    return context;
                else
                    throw new InvalidOperationException("Context not available");
            }
        }
        public static void SetDbContext(IDbContext Context)
        {
            context = Context;
        }
        /// <summary>
        /// Add Post (PostId=1, PostPluginName = PluginTextPostType, Tittle = Post1, postcontent = 1 item with PostContentId = 1)
        /// </summary>
        public static void SetPostData()
        {
            Post post = new Post();
            post.PostId = 1;
            post.PostPluginName = "PluginTextPostType";
            post.PubDate = DateTime.Now;
            post.Tittle = "Post1";
            PostContent postcontent = new PostContent();
            postcontent.PostId = 1;
            postcontent.PostContentId = 1;
            post.PostContents.Add(postcontent);

            postcontent = new PostContent();
            postcontent.PostId = 1;
            postcontent.PostContentId = 2;
            post.PostContents.Add(postcontent);

            postcontent = new PostContent();
            postcontent.PostId = 1;
            postcontent.PostContentId = 3;
            post.PostContents.Add(postcontent);

            context.Posts.Add(post);


        }
    }
}
