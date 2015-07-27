using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class InheritedAllowUnauthorizedController : AllowUnauthorizedController
    {
        [HttpGet]
        public abstract ViewResult InheritanceAction();
    }
}
