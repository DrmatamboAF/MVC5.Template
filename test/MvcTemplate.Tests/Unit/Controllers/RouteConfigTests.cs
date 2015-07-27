using MvcTemplate.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class RouteConfigTests
    {
        private RouteConfig config;

        public RouteConfigTests()
        {
            config = new RouteConfig();
        }

        #region Method: RegisterRoutes(RouteCollection routes)

        [Fact]
        public void RegisterRoutes_IgnoresAxdRoute()
        {
            RouteCollection routes = new RouteCollection();
            config.RegisterRoutes(routes);

            Route expected = new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler());
            Route actual = routes.First() as Route;

            Assert.Equal(expected.RouteHandler.GetType(), actual.RouteHandler.GetType());
            Assert.Equal(expected.Url, actual.Url);
        }

        [Fact]
        public void RegisterRoutes_RegistersDefaultMultilingualRoute()
        {
            RouteCollection routes = new RouteCollection();
            config.RegisterRoutes(routes);

            Route actual = routes["DefaultMultilingual"] as Route;

            Assert.Equal(new[] { "MvcTemplate.Controllers" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.Equal("{language}/{controller}/{action}/{id}", actual.Url);
            Assert.Equal(false, actual.DataTokens["UseNamespaceFallback"]);
            Assert.Equal(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.Equal("Home", actual.Defaults["controller"]);
            Assert.Equal("lt", actual.Constraints["language"]);
            Assert.Equal("Index", actual.Defaults["action"]);
            Assert.Null(actual.Defaults["language"]);
            Assert.Null(actual.Defaults["area"]);
        }

        [Fact]
        public void RegisterRoutes_RegistersDefaultRoute()
        {
            RouteCollection routes = new RouteCollection();
            config.RegisterRoutes(routes);

            Route actual = routes["Default"] as Route;

            Assert.Equal(new[] { "MvcTemplate.Controllers" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.Equal(false, actual.DataTokens["UseNamespaceFallback"]);
            Assert.Equal(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.Equal("{controller}/{action}/{id}", actual.Url);
            Assert.Equal("Home", actual.Defaults["controller"]);
            Assert.Equal("en", actual.Constraints["language"]);
            Assert.Equal("Index", actual.Defaults["action"]);
            Assert.Equal("en", actual.Defaults["language"]);
            Assert.Null(actual.Defaults["area"]);
        }

        #endregion
    }
}
