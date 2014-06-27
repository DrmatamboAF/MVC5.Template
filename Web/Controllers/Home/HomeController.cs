﻿using System.Web.Mvc;
using Template.Components.Security;
using Template.Resources.Shared;
using Template.Services;

namespace Template.Controllers.Home
{
    [AllowUnauthorized]
    public class HomeController : ServicedController<IAccountsService>
    {
        public HomeController(IAccountsService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            Service.AlertMessages.AddError(Messages.SystemError);

            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            Service.AlertMessages.AddError(Messages.PageNotFound);

            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            Service.AlertMessages.AddError(Messages.Unauthorized);

            return View();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
