using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Controllers
{
    [GlobalizedAuthorize]
    public abstract class BaseController : Controller
    {
        public IAuthorizationProvider AuthorizationProvider
        {
            get;
            protected set;
        }
        public virtual String CurrentAccountId
        {
            get
            {
                return User.Identity.Name;
            }
        }
        public AlertsContainer Alerts
        {
            get;
            protected set;
        }

        protected BaseController()
        {
            AuthorizationProvider = Authorization.Provider;
            Alerts = new AlertsContainer();
        }

        public virtual ActionResult NotEmptyView(Object model)
        {
            if (model == null)
                return RedirectToNotFound();

            return View(model);
        }
        public virtual ActionResult RedirectToLocal(String url)
        {
            if (!Url.IsLocalUrl(url))
                return RedirectToDefault();

            return Redirect(url);
        }
        public virtual RedirectToRouteResult RedirectToDefault()
        {
            return RedirectToAction("", "", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectToNotFound()
        {
            return RedirectToAction("NotFound", "Home", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Home", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String action)
        {
            if (!IsAuthorizedFor(action))
                return RedirectToDefault();

            return RedirectToAction(action);
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String action, Object routeValues)
        {
            RouteValueDictionary values = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            String controller = (values["controller"] ?? RouteData.Values["controller"]) as String;
            String area = (values["area"] ?? RouteData.Values["area"]) as String;

            if (!IsAuthorizedFor(area, controller, action))
                return RedirectToDefault();

            return RedirectToAction(action, routeValues);
        }

        public virtual Boolean IsAuthorizedFor(String action)
        {
            String area = (String)RouteData.Values["area"];
            String controller = (String)RouteData.Values["controller"];

            return IsAuthorizedFor(area, controller, action);
        }
        public virtual Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (AuthorizationProvider == null) return true;

            return AuthorizationProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, Object state)
        {
            String abbreviation = Request.RequestContext.RouteData.Values["language"].ToString();
            GlobalizationManager.Provider.CurrentLanguage = GlobalizationManager.Provider[abbreviation];

            return base.BeginExecuteCore(callback, state);
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!User.Identity.IsAuthenticated) return;

            String area = (String)filterContext.RouteData.Values["area"];
            String action = (String)filterContext.RouteData.Values["action"];
            String controller = (String)filterContext.RouteData.Values["controller"];

            if (!IsAuthorizedFor(area, controller, action))
                filterContext.Result = RedirectToUnauthorized();
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            AlertsContainer current = TempData["Alerts"] as AlertsContainer;
            if (current == null)
                TempData["Alerts"] = Alerts;
            else
                current.Merge(Alerts);
        }
    }
}
