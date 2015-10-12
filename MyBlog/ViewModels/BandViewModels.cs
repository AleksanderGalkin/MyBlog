using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class PostVm
    {
        public Post Post { get; set; }
        public int? PostCommentCount {
            get
            {
                if ( Post != null && Post.PostComments != null  )
                    return Post.PostComments.Count;
                else
                    return null;
            }
        }
        public int? PostViewCount {
            get
            {
                if (Post != null && Post.PostViews != null)
                    return Post.PostViews.Count;
                else
                    return null;
            }
        }
        public IEnumerable<PostContent> PostContents { get; set; }
        public IEnumerable<PostTag> PostTags { get; set; }

    }
}