using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        public string Tittle { get; set; }
        public string ApplicationUserId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PubDate { get; set; }
        public virtual ICollection<PostContent> PostContents { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<PostView> PostViews { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class PostContent
    {
        public int PostContentId { get; set; }
        public int PostId { get; set; }
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        public byte[] ContentData { get; set; }
        public ContentDataTypes ContentDataType { get; set; }
        [StringLength(100)]
        public string Comment { get; set; }
        public virtual Post Post { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public class PostView
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
    public class PostComment
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
    public class PostTag
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
    public class Tag
    {
        public int TagId { get; set; }
        [StringLength(15, MinimumLength = 2)]
        public string TagName { get; set; }

        //public virtual ICollection<Post> Posts { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public enum ContentDataTypes { Text, Image, Video};
 
}