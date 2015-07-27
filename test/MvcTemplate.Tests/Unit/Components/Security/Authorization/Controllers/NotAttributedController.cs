using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class NotAttributedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Action();
    }
}
