using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlobalizedAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RouteValueDictionary routeValues = filterContext.RouteData.Values;
            routeValues["returnUrl"] = filterContext.HttpContext.Request.RawUrl;
            routeValues["controller"] = "Auth";
            routeValues["action"] = "Login";
            routeValues["area"] = "";

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }
    }
}
