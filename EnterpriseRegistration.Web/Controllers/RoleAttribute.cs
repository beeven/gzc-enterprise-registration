using EnterpriseRegistration.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseRegistration.Web.Controllers
{
    public class RoleAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (UserProfileContext.CurrentUser.Roles.Count == 0)
            //    filterContext.Result = new RedirectResult("/Warn/Index/", true);
        }



    }
}