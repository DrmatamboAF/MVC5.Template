using MvcTemplate.Components.Security;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowAnonymous]
    public abstract class AllowAnonymousController : AuthorizedController
    {
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [AllowUnauthorized]
        public abstract ViewResult AuthorizedAction();
    }
}
