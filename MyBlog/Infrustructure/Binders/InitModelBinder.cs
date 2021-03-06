﻿using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrustructure
{
    public class InitModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType == typeof(Tuple<RegisterVm, LoginVm>))
                return new Tuple<RegisterVm, LoginVm>(new RegisterVm(), new LoginVm());
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}