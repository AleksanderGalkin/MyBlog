using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MyBlog.Infrustructure.Helpers
{
    public static  class IdentityHelperGetFullName
    {
        public static string GetFullName(this IIdentity Identity)
        {
            string result = "";
            if (Identity.IsAuthenticated)
            {
                ClaimsIdentity _claimIdentity = Identity as ClaimsIdentity;
                Claim _claim = _claimIdentity.Claims.Where(x=>x.Type == "FullName").FirstOrDefault();
                if(_claim!=null)
                {
                    result = _claim.Value.ToString();
                }
            }
            return result;
        }
    }
}