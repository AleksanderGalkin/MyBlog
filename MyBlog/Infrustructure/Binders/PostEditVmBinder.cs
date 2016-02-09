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

namespace MyBlog.Infrustructure
{
    public class PostEditVmBinder : DefaultModelBinder
    {

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ICollection<IContentType> Contents = controllerContext.HttpContext.Session["PostContents"] as Collection<IContentType>;
            if (Contents != null)
            {
                PostEditVm binding_model = bindingContext.Model as PostEditVm;
                binding_model.PostContents = binding_model.PostContents ?? new List<IContentType>();
                ICollection<IContentType> pc = binding_model.PostContents.Union(Contents).ToList();
                (bindingContext.Model as PostEditVm).PostContents = pc.ToList();
                controllerContext.HttpContext.Session["PostContents"] = null;
            }
            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}


   