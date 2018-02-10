using MyBlogContract.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Models
{
    public class Post: IPost
    {
        [HiddenInput]
        public int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        [Display(Description ="Заголовок",Name = "Заголовок")]
        public string Tittle { get; set; }
        public string ApplicationUserId { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Description = "Дата публикации",Name = "Дата публикации")]
        public DateTime PubDate { get; set; }
        
        public string PostPluginName { get; set; }

        public virtual IList<PostContent> PostContents { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual IList<PostView> PostViews { get; set; }
        public virtual IList<PostComment> PostComments { get; set; }
        public virtual IList<PostTag> PostTags { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public  Post()
        {
            PostContents = new List<PostContent>();
            PostViews = new List<PostView>();
            PostComments = new List<PostComment>();
            PostTags = new List<PostTag>();
        }
    }

    public class PostContent: IPostContent
    {
        public int PostContentId { get; set; }
        public int PostId { get; set; }
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        [AllowHtml]
        public byte[] ContentData { get; set; }

        
        public string ContentPluginName { get; set; }

        public bool IsInGroup { get; set; }
        public int Order { get; set; }

        [StringLength(100)]
        [Display(Description = "Комментарий",Name = "Комментарий")]
        public string Comment { get; set; }
        public virtual Post Post { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }


    public class PostView: IPostView
    {
        public int PostViewId { get; set; }
        public int PostId { get; set; }
        [StringLength(15, MinimumLength = 15)]
        public string Ip { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public virtual Post Post { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public class PostComment: IPostComment
    {
        public int PostCommentId { get; set; }
        public int PostId { get; set; }
        public string ApplicationUserId { get; set; }
        [StringLength(15, MinimumLength = 15)]
        public string Ip { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Post Post { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public class PostTag: IPostTag
    {
        [Key, Column(Order =0)]
        public int PostId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public class Tag: ITag
    {
        public int TagId { get; set; }
        [StringLength(15, MinimumLength = 2)]
        public string TagName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

 
}