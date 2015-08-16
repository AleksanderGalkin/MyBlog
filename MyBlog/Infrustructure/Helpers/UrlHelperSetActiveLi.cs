using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyBlog.Infrustructure.Helpers
{
    public static class UrlExtensions2
    {
        public static string SetActive(this UrlHelper urlHelper, string MatchController = null, string MatchAction = null)
        {
            string result = "active";
            if (MatchController != null)
            {
                string controller = urlHelper.RequestContext.RouteData.Values["controller"].ToString();
                result = controller.Equals(MatchController) ? result : "";
            }
            if (MatchAction != null)
            {
                string action = urlHelper.RequestContext.RouteData.Values["action"].ToString();
                result = action.Equals(MatchAction) ? result : "";
            }
            return result;
        }
    }
}