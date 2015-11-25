using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.ViewModels
{
    

    public interface IContentTypeDispVm
    {

    }

    public interface IContentTypeEditVm
    {

    }

    public class PostDispVm


    {
       // [HiddenInput]
       // public int PostId { get; set; }
        public Post Post { get; set; }
       // [StringLength(50, MinimumLength = 4)]
       // [Display(Description = "Заголовок", Name = "Заголовок")]
       // public string Tittle { get; set; }
       // public string ApplicationUserId { get; set; }
        //[DataType(DataType.DateTime)]
        //[Display(Description = "Дата публикации", Name = "Дата публикации")]
        //public DateTime PubDate { get; set; }
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
        public ICollection<IContentTypeDispVm> PostContents { get; set; }
      //  public ICollection<PostTag> PostTags { get; set; }

    }

    public class PostEditVm<T>
        where T : IContentTypeEditVm


    {
        // [HiddenInput]
        // public int PostId { get; set; }
        public Post Post { get; set; }
        // [StringLength(50, MinimumLength = 4)]
        // [Display(Description = "Заголовок", Name = "Заголовок")]
        // public string Tittle { get; set; }
        // public string ApplicationUserId { get; set; }
        //[DataType(DataType.DateTime)]
        //[Display(Description = "Дата публикации", Name = "Дата публикации")]
        //public DateTime PubDate { get; set; }
        [AllowHtml]
        public ICollection<T> PostContents { get; set; }
        //  public ICollection<PostTag> PostTags { get; set; }

    }

    public class ContentTextDispVm: IContentTypeDispVm
    {
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        public string ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }
        
    }
    public class ContentImageDispVm : IContentTypeDispVm
    {
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        public string ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }

    }

    public class ContentTextEditVm : IContentTypeEditVm
    {
        [AllowHtml]
        public string ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }
    }
    public class ContentImageEditVm : IContentTypeEditVm
    {
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        [AllowHtml]
        public string ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }

    }

}