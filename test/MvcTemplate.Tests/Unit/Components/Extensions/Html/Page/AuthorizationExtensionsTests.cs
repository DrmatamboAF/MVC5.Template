using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class AuthorizationExtensionsTests : IDisposable
    {
        private HtmlHelper html;

        public AuthorizationExtensionsTests()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            html = HtmlHelperFactory.CreateHtmlHelper();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Extension method: IsAuthorizedFor(this HtmlHelper html, String action)

        [Fact]
        public void IsAuthorizedFor_OnNullAuthorizationProviderReturnsTrue()
        {
            Authorization.Provider = null;

            Assert.True(html.IsAuthorizedFor("Create"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAuthorizedFor_ReturnsAuthorizationProviderResult(Boolean isAuthorized)
        {
            String area = html.ViewContext.RouteData.Values["area"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String controller = html.ViewContext.RouteData.Values["controller"] as String;

            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Create").Returns(isAuthorized);

            Boolean actual = html.IsAuthorizedFor("Create");
            Boolean expected = isAuthorized;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: IsAuthorizedFor(this HtmlHelper html, String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_Overload_OnNullAuthorizationProviderReturnsTrue()
        {
            Authorization.Provider = null;

            Assert.True(html.IsAuthorizedFor("Area", "Controller", "Action"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAuthorizedFor_Overload_ReturnsAuthorizationProviderResult(Boolean isAuthorized)
        {
            String area = html.ViewContext.RouteData.Values["area"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String action = html.ViewContext.RouteData.Values["action"] as String;
            String controller = html.ViewContext.RouteData.Values["controller"] as String;

            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action).Returns(isAuthorized);

            Boolean actual = html.IsAuthorizedFor(area, controller, action);
            Boolean expected = isAuthorized;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
