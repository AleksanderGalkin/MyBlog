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

namespace MyBlog.Infrustructure
{
    public class IContentTypeBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            string descriptor = bindingContext.ModelName;
            if (!String.IsNullOrEmpty(descriptor))
                descriptor = descriptor + ".";
            string availabilityContentDataType =
                GetValue(bindingContext, descriptor +"ContentDataType");
            ContentTypeEnums ContentDataType;
            if (availabilityContentDataType != null)
            {
                Enum.TryParse(availabilityContentDataType, out ContentDataType);
                IContentType model = null;
                UnicodeEncoding encoding = new UnicodeEncoding();
                switch (ContentDataType)
                {
                    case ContentTypeEnums.Text:
                        ContentTextVm tempModelText = new ContentTextVm();
                        tempModelText.ContentData = GetTextContent(bindingContext, descriptor);
                        model = tempModelText;
                        break;
                    case ContentTypeEnums.Image:
                        ContentImageVm tempModelImage = new ContentImageVm();
                        model = tempModelImage;
                        break;
                    default:
                        throw new NotImplementedException("Unknown content type: " + ContentDataType);
                }

                return model;
            }
            throw new NotImplementedException("Property ContentDataType not found");

        }


        private string GetValue(
        ModelBindingContext bindingContext, string key)
        {
            var result = bindingContext.ValueProvider.GetValue(key);
            return (result == null) ? null : result.AttemptedValue;
        }

        private string GetUnvalidatedValue(
          ModelBindingContext bindingContext, string key)
        {
            var result = bindingContext.GetValueFromValueProvider(key, false);
            return (result == null) ? null : result.AttemptedValue;
        }

        private string GetTextContent (ModelBindingContext bindingContext, string descriptor)
        {
            string result = "";
            string availabilityPostContents =
               GetUnvalidatedValue(bindingContext, descriptor + "ContentData");
            if (availabilityPostContents != null)
            {

                result = availabilityPostContents;
            }
            return result;
        }



    }
}


   