using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.ViewModels
{

    public enum PostModeEnums
    {
        Create,
        Edit,
        Delete,
        Display
    }

    public enum ContentModeEnum
    {
        Create,
        Edit,
        Delete,
        None
    }

    public interface IPost //<T>
        //where T : class,IContentType
    {
        [HiddenInput]
        PostModeEnums PostMode { get; set; }
        [HiddenInput]
        int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        [Display(Description = "Заголовок", Name = "Заголовок")]
        string Tittle { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Description = "Дата публикации", Name = "Дата публикации")]
        DateTime PubDate { get; set; }
        [AllowHtml]
        IList<IContentType> PostContents { get; set; }
    }

    public interface IContentType
    {
        ContentTypeEnums ContentDataType { get; set; }
        ContentModeEnum EditMode { get; set; }
        int PostId { get; set; }
        int PostContentId { get; set; }
        int LikePlus { get; set; }
        int LikeMinus { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        string Comment { get; set; }
        bool data_edit_diff_flag { get; set; }
        void UpdateFrom(IContentType Model);
    }

    public class PostDispVm:IPost //<T>
      //where T : class, IContentType
    {
        [HiddenInput]
        public PostModeEnums PostMode { get; set; }
        [HiddenInput]
        public int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        [Display(Description = "Заголовок", Name = "Заголовок")]
        public string Tittle { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Description = "Дата публикации", Name = "Дата публикации")]
        public DateTime PubDate { get; set; }
        public IList<IContentType> PostContents { get; set; }
        [Display(Description = "Комментарии", Name = "Комментарии")]
        public int PostCommentCount { get; set; }
        [Display(Description = "Просмотры", Name = "Просмотры")]
        public int PostViewCount { get; set; }
        public PostDispVm()
        {
            PostMode = PostModeEnums.Display;
        }

    }

    public class PostContentsFiles
    {
        public HttpPostedFileBase ContentData { get; set; }
    }
    public class PostEditVm : IPost 
    {
        [HiddenInput]
        public PostModeEnums PostMode { get; set; }
        [HiddenInput]
        public int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        [Display(Description = "Заголовок", Name = "Заголовок")]
        public string Tittle { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Description = "Дата публикации", Name = "Дата публикации")]
        public DateTime PubDate { get; set; }
        public IList<IContentType> PostContents { get; set; }
        public PostEditVm()
        {
            PostMode = PostModeEnums.Edit;
        }

    }

    public class ContentTextVm : IContentType
    {
        public ContentTypeEnums ContentDataType { get; set; }
        public ContentModeEnum EditMode { get; set; }
        public int PostId { get; set; }
        public int PostContentId { get; set; }
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        [AllowHtml]
        [Display(Description = "Содержимое", Name = "Содержимое")]
        public string ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }
        public bool data_edit_diff_flag { get; set; }
        public ContentTextVm()
        {
            ContentDataType = ContentTypeEnums.Text;
        }

        public void UpdateFrom(IContentType Model)
        {
            if (Model.GetType() != typeof(ContentTextVm))
            {
                throw new NotImplementedException("The param type must be the same type");
            }
            this.Comment = Model.Comment;
            this.ContentData = (Model as ContentTextVm).ContentData;
            this.data_edit_diff_flag = !Model.data_edit_diff_flag;
        }
    }

    public class ContentImageVm : IContentType
    {
        public ContentTypeEnums ContentDataType { get; set; }
        public ContentModeEnum EditMode { get; set; }
        public int PostId { get; set; }
        public int PostContentId { get; set; }
        public int LikePlus { get; set; }
        public int LikeMinus { get; set; }
        [AllowHtml]
        [Display(Description = "Содержимое", Name = "Содержимое")]
        public Byte[] ContentData { get; set; }
        [StringLength(100)]
        [Display(Description = "Комментарий", Name = "Комментарий")]
        public string Comment { get; set; }
        public bool data_edit_diff_flag { get; set; }
        public ContentImageVm()
        {
            ContentDataType = ContentTypeEnums.Image;
        }
        public void UpdateFrom(IContentType Model)
        {
            if (Model.GetType() != typeof(ContentImageVm))
            {
                throw new NotImplementedException("The param type must be the same type");
            }
            this.Comment = Model.Comment;
            this.data_edit_diff_flag = !Model.data_edit_diff_flag;
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string thumbnailUrl { get; set; }
        public string name { get; set; }
        public int length { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int id { get; set; }
    }

}