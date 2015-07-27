using MvcTemplate.Components.Security;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [GlobalizedAuthorize]
    public abstract class AuthorizedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Action();

        [HttpPost]
        public abstract ViewResult Action(Object obj);

        [HttpGet]
        [AllowAnonymous]
        public abstract ViewResult AllowAnonymousAction();

        [HttpGet]
        [AllowUnauthorized]
        public abstract ViewResult AllowUnauthorizedAction();
    }
}
