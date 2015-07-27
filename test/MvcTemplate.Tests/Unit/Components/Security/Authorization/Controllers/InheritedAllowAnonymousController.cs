using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class InheritedAllowAnonymousController : AllowAnonymousController
    {
        [HttpGet]
        public abstract ViewResult InheritanceAction();
    }
}
