using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyBlogContract.Search
{

    public interface IPost
    {
        int PostId { get; set; }
        string Tittle { get; set; }
        string ApplicationUserId { get; set; }
        DateTime PubDate { get; set; }
        string PostPluginName { get; set; }
       // IList<IPostTag> PostTags { get; set; }
        byte[] RowVersion { get; set; }



    }

    public interface IPostContent
    {
        int PostContentId { get; set; }
        int PostId { get; set; }
        int LikePlus { get; set; }
        int LikeMinus { get; set; }
        byte[] ContentData { get; set; }
        string ContentPluginName { get; set; }
        bool IsInGroup { get; set; }
        int Order { get; set; }
        string Comment { get; set; }
        byte[] RowVersion { get; set; }

    }


    public interface IPostView
    {
        int PostViewId { get; set; }
        int PostId { get; set; }
        string Ip { get; set; }
        DateTime Date { get; set; }
        byte[] RowVersion { get; set; }

    }
    public interface IPostComment 
    {
        int PostCommentId { get; set; }
        int PostId { get; set; }
        string ApplicationUserId { get; set; }
        string Ip { get; set; }
        DateTime Date { get; set; }
        byte[] RowVersion { get; set; }

    }
    public interface IPostTag 
    {
        int  PostId { get; set; }
        int TagId { get; set; }
        byte[]  RowVersion { get; set; }

    }
    public interface ITag 
    {
        int TagId { get; set; }
        string TagName { get; set; }
        byte[] RowVersion { get; set; }

    }


}
