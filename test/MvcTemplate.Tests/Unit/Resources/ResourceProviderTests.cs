using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using System;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Resources
{
    public class ResourceProviderTests
    {
        #region Static method: GetDatalistTitle(String datalist)

        [Fact]
        public void GetDatalistTitle_GetsTitleByIgnoringCase()
        {
            String expected = MvcTemplate.Resources.Datalist.Titles.Role;
            String actual = ResourceProvider.GetDatalistTitle("role");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDatalistTitle_OnDatalistNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetDatalistTitle("Test"));
        }

        [Fact]
        public void GetDatalistTitle_OnNullDatalistReturnsNull()
        {
            Assert.Null(ResourceProvider.GetDatalistTitle(null));
        }

        #endregion

        #region Static method: GetContentTitle(RouteValueDictionary values)

        [Fact]
        public void GetContentTitle_GetsTitleByIgnoringCase()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["area"] = "administration";
            values["controller"] = "roles";
            values["action"] = "details";

            String expected = ContentTitles.AdministrationRolesDetails;
            String actual = ResourceProvider.GetContentTitle(values);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContentTitle_GetsTitleWithoutArea(String area)
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["controller"] = "profile";
            values["action"] = "edit";
            values["area"] = area;

            String actual = ResourceProvider.GetContentTitle(values);
            String expected = ContentTitles.ProfileEdit;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentTitle_OnTitleNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetContentTitle(new RouteValueDictionary()));
        }

        #endregion

        #region Static method: GetSiteMapTitle(String area, String controller, String action)

        [Fact]
        public void GetSiteMapTitle_GetsTitleByIgnoringCase()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", "roles", "index");
            String expected = MvcTemplate.Resources.SiteMap.Titles.AdministrationRolesIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_GetsTitleWithoutControllerAndAction()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", null, null);
            String expected = MvcTemplate.Resources.SiteMap.Titles.Administration;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_OnSiteMapNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetSiteMapTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Fact]
        public void GetPrivilegeAreaTitle_GetsTitleByIgnoringCase()
        {
            String expected = MvcTemplate.Resources.Privilege.Area.Titles.Administration;
            String actual = ResourceProvider.GetPrivilegeAreaTitle("administration");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeAreaTitle_OnAreaNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        [Fact]
        public void GetPrivilegeAreaTitle_OnNullAreaReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle(null));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String area, String controller)

        [Fact]
        public void GetPrivilegeControllerTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Controller.Titles.AdministrationRoles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Administration", "Roles");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeControllerTitle_OnControllerNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeControllerTitle("", ""));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String area, String controller, String action)

        [Fact]
        public void GetPrivilegeActionTitle_GetsTitle()
        {
            String actual = ResourceProvider.GetPrivilegeActionTitle("administration", "accounts", "index");
            String expected = MvcTemplate.Resources.Privilege.Action.Titles.AdministrationAccountsIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeActionTitle_OnActionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeActionTitle("", "", ""));
        }

        #endregion

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Fact]
        public void GetPropertyTitle_OnNotMemberExpressionReturnNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Username);
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromExpressionRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountEditView, String>(account => account.RoleId);
            String expected = MvcTemplate.Resources.Views.RoleView.Titles.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_OnPropertyFromExpressionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Id));
        }

        [Fact]
        public void GetPropertyTitle_OnTypeFromExpressionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(test => test.Text));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type view, String property)

        [Fact]
        public void GetPropertyTitle_GetsTitleByIgnoringCase()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(AccountView), "username");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromMultipleRelations()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountRoleAccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_OnPropertyNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(AccountView), "Id"));
        }

        [Fact]
        public void GetPropertyTitle_OnTypeNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(TestView), "Title"));
        }

        [Fact]
        public void GetPropertyTitle_OnNullPropertyKeyReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(RoleView), null));
        }

        #endregion
    }
}
