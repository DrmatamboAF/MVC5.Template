using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security.Area
{
    [Area("Area")]
    [GlobalizedAuthorize]
    public abstract class AuthorizedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Action();

        [HttpPost]
        public abstract ViewResult Action(Object obj);

        [HttpGet]
        public abstract ViewResult AuthorizedGetAction();

        [HttpPost]
        [AllowAnonymous]
        public abstract ViewResult AuthorizedGetAction(Object obj);

        [HttpPost]
        public abstract ViewResult AuthorizedPostAction();

        [HttpGet]
        [ActionName("AuthorizedNamedGetAction")]
        public abstract ViewResult GetActionWithName();

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AuthorizedNamedGetAction")]
        public abstract ViewResult GetActionWithName(Object obj);

        [HttpPost]
        [ActionName("AuthorizedNamedPostAction")]
        public abstract ViewResult PostActionWithName();

        [HttpGet]
        public abstract ViewResult NotAuthorizedAs();

        [HttpGet]
        [AuthorizeAs("AuthorizedAsItself")]
        public abstract ViewResult AuthorizedAsItself();

        [HttpGet]
        [AuthorizeAs("Action")]
        public abstract ViewResult AuthorizedAsAction();
    }
}
