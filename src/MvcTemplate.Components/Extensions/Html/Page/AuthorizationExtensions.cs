using MvcTemplate.Components.Security;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class AuthorizationExtensions
    {
        public static Boolean IsAuthorizedFor(this HtmlHelper html, String action)
        {
            if (Authorization.Provider == null)
                return true;

            String area = html.ViewContext.RouteData.Values["area"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String controller = html.ViewContext.RouteData.Values["controller"] as String;

            return Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action);
        }
        public static Boolean IsAuthorizedFor(this HtmlHelper html, String area, String controller, String action)
        {
            if (Authorization.Provider == null)
                return true;

            String accountId = html.ViewContext.HttpContext.User.Identity.Name;

            return Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action);
        }
    }
}
