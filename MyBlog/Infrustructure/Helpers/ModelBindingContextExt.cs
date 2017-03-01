using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrustructure.Helpers
{
    public static class ModelBindingContextExt
    {
        public static ValueProviderResult GetValueFromValueProvider(this ModelBindingContext bindingContext, string key, bool performRequestValidation)
        {
            var unvalidatedValueProvider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            return (unvalidatedValueProvider != null)
               ? unvalidatedValueProvider.GetValue(key, !performRequestValidation)
               : bindingContext.ValueProvider.GetValue(key);
        }
    }
}