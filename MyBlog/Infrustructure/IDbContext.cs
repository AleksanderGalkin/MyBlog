using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.Linq;
using System.Web;

namespace MyBlog.Infrustructure
{
    public interface IDbContext:IDisposable
    {
        IDbSet<Post> Posts { get; set; }
        IDbSet<PostContent> PostContents { get; set; }
        IDbSet<PostView> PostViews { get; set; }
        IDbSet<PostComment> PostComments { get; set; }
        IDbSet<PostTag> PostTags { get; set; }
        IDbSet<Tag> Tags { get; set; }

        int SaveChanges();
        
    }
}