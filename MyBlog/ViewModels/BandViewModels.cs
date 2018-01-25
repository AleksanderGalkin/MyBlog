using MyBlog.Models;
using MyBlogContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.ViewModels
{

    public interface IPost 
    {

        int PostId { get; set; }
        string Tittle { get; set; }
        DateTime PubDate { get; set; }

        string PostPluginName { get; set; }

        IList<IDataStoreRecord> PostContents { get; set; }
    }

    public class PostVm:IPost 
    {

        [HiddenInput]
        public int PostId { get; set; }
        [StringLength(50, MinimumLength = 4)]
        [Display(Description = "Заголовок", Name = "Заголовок")]
        public string Tittle { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Description = "Дата публикации", Name = "Дата публикации")]
        public DateTime PubDate { get; set; }
        public string PostPluginName { get; set; }
        public IList<IDataStoreRecord> PostContents { get; set; }
        [Display(Description = "Комментарии", Name = "Комментарии")]
        public int PostCommentCount { get; set; }
        [Display(Description = "Просмотры", Name = "Просмотры")]
        public int PostViewCount { get; set; }


        public string TagSession { get; set; }




    }

    public class PostGroupVm : PostVm
    {
        public IList<GroupVmDisplay> GroupPostContents { get; set; }

    }

    public class GroupVmDisplay
    {
        public int PostId { get; set; }
        public int Order { get; set; }
        public string GroupPluginName { get; set; }
        public IEnumerable<IDataStoreRecord> Contexts { get; set; }
    }

    public class PostContentsFiles
    {
        public HttpPostedFileBase ContentData { get; set; }
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

    public class VmTag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public bool Select { get; set; }

    }

}