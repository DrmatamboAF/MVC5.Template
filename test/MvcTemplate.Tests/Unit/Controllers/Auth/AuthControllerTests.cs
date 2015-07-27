using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mail;
using MvcTemplate.Controllers;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class AuthControllerTests : AControllerTests
    {
        private AccountRegisterView accountRegister;
        private AccountRecoveryView accountRecovery;
        private AccountResetView accountReset;
        private AccountLoginView accountLogin;
        private IAccountValidator validator;
        private AuthController controller;
        private IAccountService service;
        private IMailClient mailClient;

        public AuthControllerTests()
        {
            mailClient = Substitute.For<IMailClient>();
            service = Substitute.For<IAccountService>();
            validator = Substitute.For<IAccountValidator>();
            controller = Substitute.ForPartsOf<AuthController>(validator, service, mailClient);

            accountRegister = ObjectFactory.CreateAccountRegisterView();
            accountRecovery = ObjectFactory.CreateAccountRecoveryView();
            accountReset = ObjectFactory.CreateAccountResetView();
            accountLogin = ObjectFactory.CreateAccountLoginView();

            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            controller.Url = new UrlHelper(httpContext.Request.RequestContext);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
        }

        #region Method: Register()

        [Fact]
        public void Register_RedirectsToDefaultIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Register();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_IfNotLoggedInReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            Object model = (controller.Register() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Register(AccountRegisterView account)

        [Fact]
        public void Register_ProtectsFromOverpostingId()
        {
            ProtectsFromOverposting(controller, "Register", "Id");
        }

        [Fact]
        public void Register_OnPostRedirectsToDefaultIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Register(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_ReturnsSameModelIfCanNotRegister()
        {
            validator.CanRegister(accountRegister).Returns(false);
            service.IsLoggedIn(controller.User).Returns(false);

            Object actual = (controller.Register(accountRegister) as ViewResult).Model;
            Object expected = accountRegister;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_RegistersAccount()
        {
            validator.CanRegister(accountRegister).Returns(true);
            service.IsLoggedIn(controller.User).Returns(false);

            controller.Register(accountRegister);

            service.Received().Register(accountRegister);
        }

        [Fact]
        public void Register_AddsSuccessfulRegistrationMessage()
        {
            validator.CanRegister(accountRegister).Returns(true);
            service.IsLoggedIn(controller.User).Returns(false);

            controller.Register(accountRegister);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.Equal(Messages.SuccessfulRegistration, actual.Message);
            Assert.Equal(AlertType.Success, actual.Type);
        }

        [Fact]
        public void Register_AfterRegistrationRedirectsToLogin()
        {
            validator.CanRegister(accountRegister).Returns(true);
            service.IsLoggedIn(controller.User).Returns(false);

            RouteValueDictionary actual = (controller.Register(accountRegister) as RedirectToRouteResult).RouteValues;

            Assert.Equal("Login", actual["action"]);
            Assert.Equal(1, actual.Count);
        }

        #endregion

        #region Method: Recover()

        [Fact]
        public void Recover_RedirectsToDefaultIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Recover();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_IfNotLoggedInReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            Object model = (controller.Recover() as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Recover(AccountRecoveryView account)

        [Fact]
        public void Recover_OnAlreadyLoggedInRedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            validator.CanRecover(accountRecovery).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Recover(null).Result;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_ReturnsSameModelIfCanNotRecover()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(false);

            Object actual = (controller.Recover(accountRecovery).Result as ViewResult).Model;
            Object expected = accountRecovery;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_RecoversAccount()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);

            ActionResult result = controller.Recover(accountRecovery).Result;

            service.Received().Recover(accountRecovery);
        }

        [Fact]
        public void Recover_OnNotNullRecoveryTokenSendsRecoveryInformation()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns("TestToken");

            ActionResult result = controller.Recover(accountRecovery).Result;

            String url = controller.Url.Action("Reset", "Auth", new { token = "TestToken" }, controller.Request.Url.Scheme);
            String body = String.Format(Messages.RecoveryEmailBody, url);
            String subject = Messages.RecoveryEmailSubject;
            String email = accountRecovery.Email;

            mailClient.Received().SendAsync(email, subject, body);
        }

        [Fact]
        public void Recover_OnNullRecoveryTokenDoesNotSendRecoveryInformation()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns(null as String);

            ActionResult result = controller.Recover(accountRecovery).Result;

            mailClient.DidNotReceive().SendAsync(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Token")]
        public void Recover_AddsRecoveryInformationMessage(String recoveryToken)
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns(recoveryToken);

            ActionResult result = controller.Recover(accountRecovery).Result;

            Alert actual = controller.Alerts.Single();

            Assert.Equal(Messages.RecoveryInformation, actual.Message);
            Assert.Equal(AlertType.Info, actual.Type);
            Assert.Equal(0, actual.FadeoutAfter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Token")]
        public void Recover_AfterRecoveryRedirectsToLogin(String recoveryToken)
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns(recoveryToken);

            RouteValueDictionary actual = (controller.Recover(accountRecovery).Result as RedirectToRouteResult).RouteValues;

            Assert.Equal("Login", actual["action"]);
            Assert.Equal(1, actual.Count);
        }

        #endregion

        #region Method: Reset(String token)

        [Fact]
        public void Reset_RedirectsToDefaultIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Reset("");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Reset_RedirectsToRecoverIfCanNotReset()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(Arg.Any<AccountResetView>()).Returns(false);

            RouteValueDictionary actual = (controller.Reset("Token") as RedirectToRouteResult).RouteValues;

            Assert.Equal("Recover", actual["action"]);
            Assert.Equal(1, actual.Count);
        }

        [Fact]
        public void Reset_IfCanResetReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(Arg.Any<AccountResetView>()).Returns(true);

            Object model = (controller.Reset("") as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Reset(AccountResetView account)

        [Fact]
        public void Reset_OnPostRedirectsToDefaultIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Reset(accountReset);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Reset_OnPostRedirectsToRecoverIfCanNotReset()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(false);

            RouteValueDictionary actual = (controller.Reset(accountReset) as RedirectToRouteResult).RouteValues;

            Assert.Equal("Recover", actual["action"]);
            Assert.Equal(1, actual.Count);
        }

        [Fact]
        public void Reset_ResetsAccount()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            service.Received().Reset(accountReset);
        }

        [Fact]
        public void Reset_AddsSuccessfulResetMessage()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.Equal(Messages.SuccessfulReset, actual.Message);
            Assert.Equal(AlertType.Success, actual.Type);
        }

        [Fact]
        public void Reset_AfterResetRedirectsToLogin()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            RouteValueDictionary actual = (controller.Reset(accountReset) as RedirectToRouteResult).RouteValues;

            Assert.Equal("Login", actual["action"]);
            Assert.Equal(1, actual.Count);
        }

        #endregion

        #region Method: Login(String returnUrl)

        [Fact]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            ActionResult expected = controller.RedirectToLocal("/");
            ActionResult actual = controller.Login("/");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_IfNotLoggedInReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            Object model = (controller.Login("/") as ViewResult).Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Login(AccountLoginView account, String returnUrl)

        [Fact]
        public void Login_OnPostRedirectsToUrlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            ActionResult expected = controller.RedirectToLocal("/");
            ActionResult actual = controller.Login(null, "/");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_ReturnsSameModelIfCanNotLogin()
        {
            validator.CanLogin(accountLogin).Returns(false);

            Object actual = (controller.Login(accountLogin, null) as ViewResult).Model;
            Object expected = accountLogin;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_LogsInAccount()
        {
            validator.CanLogin(accountLogin).Returns(true);
            controller.When(sub => sub.RedirectToLocal(null)).DoNotCallBase();
            controller.RedirectToLocal(null).Returns(new RedirectResult("/"));

            controller.Login(accountLogin, null);

            service.Received().Login(accountLogin.Username);
        }

        [Fact]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            validator.CanLogin(accountLogin).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            ActionResult actual = controller.Login(accountLogin, "/");
            ActionResult expected = controller.RedirectToLocal("/");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Fact]
        public void Logout_LogsOut()
        {
            controller.Logout();

            service.Received().Logout();
        }

        [Fact]
        public void Logout_RedirectsToLogin()
        {
            Object actual = controller.Logout().RouteValues["action"];
            Object expected = "Login";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
