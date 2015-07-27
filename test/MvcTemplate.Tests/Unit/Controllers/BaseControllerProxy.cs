using MvcTemplate.Controllers;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerProxy : BaseController
    {
        public RedirectToRouteResult BaseRedirectToAction(String action)
        {
            return RedirectToAction(action);
        }
        public RedirectToRouteResult BaseRedirectToAction(String action, Object routeValues)
        {
            return RedirectToAction(action, routeValues);
        }

        public IAsyncResult BaseBeginExecuteCore(AsyncCallback callback, Object state)
        {
            return BeginExecuteCore(callback, state);
        }
        public void BaseOnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorization(filterContext);
        }
        public void BaseOnActionExecuted(ActionExecutedContext filterContext)
        {
            OnActionExecuted(filterContext);
        }
    }
}
