using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerTests : IDisposable
    {
        private BaseControllerProxy controller;

        public BaseControllerTests()
        {
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            controller = Substitute.ForPartsOf<BaseControllerProxy>();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
            controller.ControllerContext.Controller = controller;
            controller.ControllerContext.RouteData =
                httpContext.Request.RequestContext.RouteData;
            controller.Url = Substitute.For<UrlHelper>();
        }
        public void Dispose()
        {
            GlobalizationManager.Provider = null;
            Authorization.Provider = null;
        }

        #region Property: CurrentAccountId

        [Fact]
        public void CurrentAccountId_GetsCurrentIdentityName()
        {
            String expected = controller.User.Identity.Name;
            String actual = controller.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: BaseController()

        [Fact]
        public void BaseController_SetsAuthorizationProviderFromFactory()
        {
            IAuthorizationProvider actual = controller.AuthorizationProvider;
            IAuthorizationProvider expected = Authorization.Provider;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseController_CreatesEmptyAlertsContainer()
        {
            Assert.Empty(controller.Alerts);
        }

        #endregion

        #region Method: NotEmptyView(Object model)

        [Fact]
        public void NotEmptyView_RedirectsToNotFoundIfModelIsNull()
        {
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToNotFound();
            ActionResult actual = controller.NotEmptyView(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void NotEmptyView_ReturnsEmptyView()
        {
            Object expected = new Object();
            Object actual = (controller.NotEmptyView(expected) as ViewResult).Model;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Fact]
        public void RedirectToLocal_RedirectsToDefaultIfUrlIsNotLocal()
        {
            controller.Url.IsLocalUrl("www.test.com").Returns(false);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult actual = controller.RedirectToLocal("www.test.com");
            ActionResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectToLocal_RedirectsToLocalIfUrlIsLocal()
        {
            controller.Url.IsLocalUrl("/").Returns(true);

            String actual = (controller.RedirectToLocal("/") as RedirectResult).Url;
            String expected = "/";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Fact]
        public void RedirectToDefault_RedirectsToDefault()
        {
            RouteValueDictionary actual = controller.RedirectToDefault().RouteValues;

            Assert.Equal("", actual["controller"]);
            Assert.Equal("", actual["action"]);
            Assert.Equal("", actual["area"]);
        }

        #endregion

        #region Method: RedirectToNotFound()

        [Fact]
        public void RedirectToNotFound_RedirectsToNotFound()
        {
            RouteValueDictionary actual = controller.RedirectToNotFound().RouteValues;

            Assert.Equal("NotFound", actual["action"]);
            Assert.Equal("Home", actual["controller"]);
            Assert.Equal("", actual["area"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Fact]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            RouteValueDictionary actual = controller.RedirectToUnauthorized().RouteValues;

            Assert.Equal("Unauthorized", actual["action"]);
            Assert.Equal("Home", actual["controller"]);
            Assert.Equal("", actual["area"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Fact]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            controller.IsAuthorizedFor("Action").Returns(false);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            RedirectToRouteResult actual = controller.RedirectIfAuthorized("Action");
            RedirectToRouteResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
        {
            controller.IsAuthorizedFor("Action").Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action").RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action").RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action, Object routeValues)

        [Fact]
        public void RedirectIfAuthorized_RouteValues_RedirectsToDefaultIfNotAuthorizedWithSpecifiedRoute()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);

            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            RedirectToRouteResult actual = controller.RedirectIfAuthorized("Action", new { controller = "Control", area = "Area" });
            RedirectToRouteResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RouteValues_RedirectsToDefaultIfNotAuthorized()
        {
            String areaRoute = controller.RouteData.Values["area"] as String;
            String controllerRoute = controller.RouteData.Values["controller"] as String;
            controller.IsAuthorizedFor(areaRoute, controllerRoute, "Action").Returns(false);

            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            RedirectToRouteResult actual = controller.RedirectIfAuthorized("Action", new { id = "Id" });
            RedirectToRouteResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RouteValues_RedirectsToActionIfAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Control", "Action").Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", new { controller = "Control", area = "Area", id = "Id" }).RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", new { controller = "Control", area = "Area", id = "Id" }).RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
            Assert.Equal(expected["id"], actual["id"]);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action)

        [Fact]
        public void IsAuthorizedFor_ReturnsTrueThenAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(true);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.True(controller.IsAuthorizedFor("Action"));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsFalseThenNotAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.False(controller.IsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_OnNullAuthorizationProviderReturnsTrue()
        {
            Authorization.Provider = null;
            controller = Substitute.ForPartsOf<BaseControllerProxy>();

            Assert.Null(controller.AuthorizationProvider);
            Assert.True(controller.IsAuthorizedFor(null, null, null));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsAuthorizationProviderResult()
        {
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "AR", "CO", "AC").Returns(true);

            Assert.True(controller.IsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion

        #region Method: BeginExecuteCore(AsyncCallback callback, Object state)

        [Fact]
        public void BeginExecuteCore_SetsLangaugeFromRequestsRouteValues()
        {
            controller.RouteData.Values["language"] = "lt";
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            GlobalizationManager.Provider.CurrentLanguage = GlobalizationManager.Provider["en"];

            controller.BaseBeginExecuteCore(asyncResult => { }, null);

            Language actual = GlobalizationManager.Provider.CurrentLanguage;
            Language expected = GlobalizationManager.Provider["lt"];

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: OnAuthorization(AuthorizationContext filterContext)

        [Fact]
        public void OnAuthorization_SetsResultToNullThenNotLoggedIn()
        {
            ActionDescriptor describtor = Substitute.ForPartsOf<ActionDescriptor>();
            AuthorizationContext filterContext = new AuthorizationContext(controller.ControllerContext, describtor);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(false);

            controller.BaseOnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_SetsResultToRedirectToUnauthorizedIfNotAuthorized()
        {
            controller.When(sub => sub.RedirectToUnauthorized()).DoNotCallBase();
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            controller.RedirectToUnauthorized().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            controller.BaseOnAuthorization(context);

            ActionResult expected = controller.RedirectToUnauthorized();
            ActionResult actual = context.Result;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnAuthorization_SetsResultToNullThenAuthorized()
        {
            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "Area", "Controller", "Action").Returns(true);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            context.RouteData.Values["controller"] = "Controller";
            context.RouteData.Values["action"] = "Action";
            context.RouteData.Values["area"] = "Area";

            controller.BaseOnAuthorization(context);

            Assert.Null(context.Result);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext filterContext)

        [Fact]
        public void OnActionExecuted_SetsAlertsToTempDataThenAlertsInTempDataAreNull()
        {
            controller.TempData["Alerts"] = null;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Object actual = controller.TempData["Alerts"];
            Object expected = controller.Alerts;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void OnActionExecuted_MergesAlertsToTempData()
        {
            HttpContextBase context = controller.HttpContext;

            BaseControllerProxy mergedController = new BaseControllerProxy();
            mergedController.ControllerContext = new ControllerContext();
            mergedController.ControllerContext.HttpContext = context;
            mergedController.TempData = controller.TempData;
            mergedController.Alerts.AddError("ErrorTest2");

            IEnumerable<Alert> controllerAlerts = controller.Alerts;
            IEnumerable<Alert> mergedAlerts = mergedController.Alerts;

            controller.Alerts.AddError("ErrorTest1");
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            mergedController.BaseOnActionExecuted(new ActionExecutedContext());

            IEnumerable<Alert> actual = controller.TempData["Alerts"] as AlertsContainer;
            IEnumerable<Alert> expected = controllerAlerts.Union(mergedAlerts);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
