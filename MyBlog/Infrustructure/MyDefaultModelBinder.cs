using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrustructure
{
    public class MyDefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType == typeof(Tuple<LandingRegisterViewModel, LandingLoginViewModel>))
                return new Tuple<LandingRegisterViewModel, LandingLoginViewModel>(new LandingRegisterViewModel(), new LandingLoginViewModel());
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}