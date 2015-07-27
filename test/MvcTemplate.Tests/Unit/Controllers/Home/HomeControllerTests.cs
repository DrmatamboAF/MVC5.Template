using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class HomeControllerTests
    {
        private HomeController controller;
        private IAccountService service;

        public HomeControllerTests()
        {
            service = Substitute.For<IAccountService>();
            controller = Substitute.ForPartsOf<HomeController>(service);

            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns("CurrentAccount");
        }

        #region Method: Index()

        [Fact]
        public void Index_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Index() as RedirectToRouteResult;

            Assert.Equal("Auth", actual.RouteValues["controller"]);
            Assert.Equal("Logout", actual.RouteValues["action"]);
            Assert.Equal(2, actual.RouteValues.Count);
        }

        [Fact]
        public void Index_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object model = (controller.Index() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Error()

        [Fact]
        public void Error_ReturnsEmptyView()
        {
            Object model = (controller.Error() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: NotFound()

        [Fact]
        public void NotFound_ReturnsEmptyView()
        {
            Object model = (controller.NotFound() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Unauthorized()

        [Fact]
        public void Unauthorized_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Unauthorized() as RedirectToRouteResult;

            Assert.Equal("Auth", actual.RouteValues["controller"]);
            Assert.Equal("Logout", actual.RouteValues["action"]);
            Assert.Equal(2, actual.RouteValues.Count);
        }

        [Fact]
        public void Unauthorized_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object model = (controller.Unauthorized() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion
    }
}
