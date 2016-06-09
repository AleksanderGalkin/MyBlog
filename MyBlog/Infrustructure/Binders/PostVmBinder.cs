using MyBlog.Infrustructure.Helpers;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MyBlogContract;

namespace MyBlog.Infrustructure
{
    public class PostVmBinder : DefaultModelBinder
    {

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IList<IDataStoreRecord> Contents = controllerContext.HttpContext.Session["PostContents"] as IList<IDataStoreRecord>;
            if (Contents != null)
            {
                PostVm binding_model = bindingContext.Model as PostVm;
                binding_model.PostContents = binding_model.PostContents ?? new List<IDataStoreRecord>();
                IList<IDataStoreRecord> pc = binding_model.PostContents.Union(Contents).ToList();
                (bindingContext.Model as PostVm).PostContents = pc.ToList();
                controllerContext.HttpContext.Session["PostContents"] = null;
            }
            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}


   