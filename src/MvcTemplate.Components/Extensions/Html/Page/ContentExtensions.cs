using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class ContentExtensions
    {
        public static MvcHtmlString RenderControllerScript(this HtmlHelper html)
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            String controller = routeValues["controller"].ToString();
            String scriptDir = controller;

            if (routeValues["Area"] != null)
                scriptDir = routeValues["Area"] + "/" + scriptDir;

            String virtualPath = urlHelper.Content(String.Format("~/Scripts/Shared/{0}/{1}.js", scriptDir, controller.ToLower()));
            String physicalPath = html.ViewContext.HttpContext.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return MvcHtmlString.Empty;

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("src", virtualPath);

            return new MvcHtmlString(script.ToString());
        }
        public static MvcHtmlString RenderControllerStyle(this HtmlHelper html)
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            String controller = routeValues["controller"].ToString();
            String styleDir = controller;

            if (routeValues["Area"] != null)
                styleDir = routeValues["Area"] + "/" + styleDir;

            String virtualPath = urlHelper.Content(String.Format("~/Content/Shared/{0}/{1}.css", styleDir, controller.ToLower()));
            String physicalPath = html.ViewContext.HttpContext.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return MvcHtmlString.Empty;

            TagBuilder link = new TagBuilder("link");
            link.MergeAttribute("href", virtualPath);
            link.MergeAttribute("rel", "stylesheet");

            return new MvcHtmlString(link.ToString());
        }
    }
}
