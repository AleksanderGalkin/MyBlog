using MainApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrastructure.Services
{
    public class DemBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            object model = null;
            object area_variable = controllerContext.RouteData.DataTokens["area"];
            string current_area = "";
            if (area_variable != null)
            {
                current_area = controllerContext.RouteData.DataTokens["area"].ToString();
            }

            var UrlReferrer = controllerContext.HttpContext.Request.UrlReferrer;
            string second_segment_prev_area = "";
            if (UrlReferrer != null && controllerContext.HttpContext.Request.UrlReferrer.Segments.Count()>1)
            {
                second_segment_prev_area = controllerContext.HttpContext.Request.UrlReferrer.Segments[1];
            }
            int length_second_segment_prev_area = second_segment_prev_area.Length;
            if (length_second_segment_prev_area>0)
            {
                second_segment_prev_area = second_segment_prev_area.Substring(0, length_second_segment_prev_area - 1);
            }
            string previous_area = "";
            if (PlugInFactory.GetPluginNamesList().Contains(second_segment_prev_area))
            {
                previous_area = second_segment_prev_area;
            }
            if ( ! string.IsNullOrWhiteSpace(current_area))
            {
                model = PlugInFactory.GetModelByInterface(modelType, current_area);
            } else
            if (!string.IsNullOrWhiteSpace(previous_area))
            {
                model = PlugInFactory.GetModelByInterface(modelType, previous_area);
            }
            return model ?? base.CreateModel(controllerContext, bindingContext, modelType);
        }

        
    }
}