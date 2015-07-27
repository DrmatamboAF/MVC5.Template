using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class GlobalizedAuthorizeAttributeTests
    {
        #region Method: HandleUnauthorizedRequest(AuthorizationContext filterContext)

        [Fact]
        public void HandleUnauthorizedRequest_RedirectsToLogin()
        {
            GlobalizedAuthorizeAttributeProxy attribute = new GlobalizedAuthorizeAttributeProxy();
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            AuthorizationContext context = new AuthorizationContext();

            context.RouteData = httpContext.Request.RequestContext.RouteData;
            context.HttpContext = httpContext;

            attribute.BaseHandleUnauthorizedRequest(context);

            RouteValueDictionary actual = (context.Result as RedirectToRouteResult).RouteValues;

            Assert.Equal(context.RouteData.Values["language"], actual["language"]);
            Assert.Equal(context.HttpContext.Request.RawUrl, actual["returnUrl"]);
            Assert.Equal("Auth", actual["controller"]);
            Assert.Equal("Login", actual["action"]);
            Assert.Equal("", actual["area"]);
            Assert.Equal(5, actual.Count);
        }

        #endregion
    }
}
