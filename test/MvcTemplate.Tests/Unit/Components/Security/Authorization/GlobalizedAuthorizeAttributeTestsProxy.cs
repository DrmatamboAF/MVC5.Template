using MvcTemplate.Components.Security;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class GlobalizedAuthorizeAttributeProxy : GlobalizedAuthorizeAttribute
    {
        public void BaseHandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HandleUnauthorizedRequest(filterContext);
        }
    }
}
