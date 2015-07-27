using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MvcSiteMapProviderTests : IDisposable
    {
        private static MvcSiteMapParser parser;
        private static String siteMapPath;

        private RouteValueDictionary routeValues;
        private MvcSiteMapProvider provider;
        private ViewContext viewContext;

        static MvcSiteMapProviderTests()
        {
            siteMapPath = "MvcSiteMapProviderTests.sitemap";
            parser = new MvcSiteMapParser();
            CreateSiteMap(siteMapPath);
        }
        public MvcSiteMapProviderTests()
        {
            viewContext = HtmlHelperFactory.CreateHtmlHelper().ViewContext;
            provider = new MvcSiteMapProvider(siteMapPath, parser);
            routeValues = viewContext.RouteData.Values;
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Method: GetAuthorizedMenus(ViewContext context)

        [Fact]
        public void GetAuthorizedMenus_OnNullAuthorizationProviderReturnsAllMenus()
        {
            Authorization.Provider = null;

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);

            Assert.Equal(0, actual[0].Children.Count());

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);

            Assert.Null(actual[1].Action);
            Assert.Equal("Roles", actual[1].Controller);
            Assert.Equal("Administration", actual[1].Area);
            Assert.Equal("fa fa-users", actual[1].IconClass);

            actual = actual[1].Children.ToArray();

            Assert.Equal(1, actual.Length);
            Assert.Equal(0, actual[0].Children.Count());

            Assert.Equal("Create", actual[0].Action);
            Assert.Equal("Roles", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-file-o", actual[0].IconClass);
        }

        [Fact]
        public void GetAuthorizedMenus_ReturnsOnlyAuthorizedMenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(viewContext.HttpContext.User.Identity.Name, "Administration", "Accounts", "Index").Returns(true);

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Equal(1, actual.Length);

            Assert.Equal(0, actual[0].Children.Count());

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);
        }

        [Fact]
        public void GetAuthorizedMenus_SetsActiveMenu()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Create";
            routeValues["controller"] = "Roles";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.False(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.False(actual[0].IsActive);
            Assert.False(actual[1].IsActive);
            Assert.Equal(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.True(actual[0].IsActive);
            Assert.Equal(1, actual.Length);
            Assert.Equal(0, actual[0].Children.Count());
        }

        [Fact]
        public void GetAuthorizedMenus_SetsActiveMenuIfNonMenuChildrenNodeIsActive()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Edit";
            routeValues["controller"] = "Accounts";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.False(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.True(actual[0].IsActive);
            Assert.False(actual[1].IsActive);
            Assert.Equal(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.Equal(1, actual.Length);
            Assert.False(actual[0].IsActive);
            Assert.Equal(0, actual[0].Children.Count());
        }

        [Fact]
        public void GetAuthorizedMenus_SetsHasActiveChildrenOnEveryActiveMenuParents()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Create";
            routeValues["controller"] = "Roles";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.True(actual[0].HasActiveChildren);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.True(actual[1].HasActiveChildren);
            Assert.False(actual[0].HasActiveChildren);
            Assert.Equal(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.Equal(1, actual.Length);
            Assert.False(actual[0].HasActiveChildren);
            Assert.Equal(0, actual[0].Children.Count());
        }

        [Fact]
        public void GetAuthorizedMenus_RemovesEmptyMenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>()).Returns(true);
            Authorization.Provider.IsAuthorizedFor(viewContext.HttpContext.User.Identity.Name, "Administration", "Roles", "Create").Returns(false);

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus(viewContext).ToArray();

            Assert.Equal(1, actual.Length);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Equal(1, actual.Length);

            Assert.Equal(0, actual[0].Children.Count());

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);
        }

        #endregion

        #region Method: GetBreadcrumb(ViewContext context)

        [Fact]
        public void GetBreadcrumb_FormsBreadcrumbByIgnoringCase()
        {
            routeValues["controller"] = "profile";
            routeValues["action"] = "edit";
            routeValues["area"] = null;

            MvcSiteMapNode[] actual = provider.GetBreadcrumb(viewContext).ToArray();

            Assert.Equal(3, actual.Length);

            Assert.Equal("fa fa-home", actual[0].IconClass);
            Assert.Equal("Home", actual[0].Controller);
            Assert.Equal("Index", actual[0].Action);
            Assert.Null(actual[0].Area);

            Assert.Equal("fa fa-user", actual[1].IconClass);
            Assert.Equal("Profile", actual[1].Controller);
            Assert.Null(actual[1].Action);
            Assert.Null(actual[1].Area);

            Assert.Equal("fa fa-pencil", actual[2].IconClass);
            Assert.Equal("Profile", actual[2].Controller);
            Assert.Equal("Edit", actual[2].Action);
            Assert.Null(actual[2].Area);
        }

        [Fact]
        public void GetBreadcrumb_OnNotFoundCurrentActionReturnsEmpty()
        {
            routeValues["controller"] = "profile";
            routeValues["action"] = "edit";
            routeValues["area"] = "area";

            Assert.Empty(provider.GetBreadcrumb(viewContext));
        }

        #endregion

        #region Test helpers

        private static void CreateSiteMap(String path)
        {
            XElement
                .Parse(
                    @"<siteMap>
                        <siteMapNode icon=""fa fa-home"" controller=""Home"" action=""Index"">
                            <siteMapNode icon=""fa fa-user"" controller=""Profile"">
                                <siteMapNode icon=""fa fa-pencil"" controller=""Profile"" action=""Edit"" />
                            </siteMapNode>
                            <siteMapNode menu=""true"" icon=""fa fa-gears"" area=""Administration"">
                                <siteMapNode menu=""true"" icon=""fa fa-user"" area=""Administration"" controller=""Accounts"" action=""Index"">
                                    <siteMapNode icon=""fa fa-info"" area=""Administration"" controller=""Accounts"" action=""Details"">
                                        <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Accounts"" action=""Edit"" />
                                    </siteMapNode>
                                </siteMapNode>
                                <siteMapNode menu=""true"" icon=""fa fa-users"" area=""Administration"" controller=""Roles"">
                                    <siteMapNode menu=""true"" icon=""fa fa-file-o"" area=""Administration"" controller=""Roles"" action=""Create"" />
                                    <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Roles"" action=""Edit"" />
                                </siteMapNode>
                            </siteMapNode>
                        </siteMapNode>
                    </siteMap>")
                .Save(path);
        }

        #endregion
    }
}
