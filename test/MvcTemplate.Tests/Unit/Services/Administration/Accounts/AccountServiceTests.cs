using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Services
{
    public class AccountServiceTests : IDisposable
    {
        private AccountService service;
        private TestingContext context;
        private Account account;
        private IHasher hasher;

        public AccountServiceTests()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            hasher.HashPassword(Arg.Any<String>()).Returns(info => info.Arg<String>() + "Hashed");

            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            service = new AccountService(new UnitOfWork(context), hasher);

            TearDownData();
            SetUpData();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
            service.Dispose();
            context.Dispose();
        }

        #region Method: Get<TView>(String id)

        [Fact]
        public void GetView_GetsViewById()
        {
            AccountView actual = service.Get<AccountView>(account.Id);
            AccountView expected = Mapper.Map<AccountView>(account);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.RoleTitle, actual.RoleTitle);
            Assert.Equal(expected.IsLocked, actual.IsLocked);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: GetViews()

        [Fact]
        public void GetViews_GetsAccountViews()
        {
            IEnumerator<AccountView> actual = service.GetViews().GetEnumerator();
            IEnumerator<AccountView> expected = context
                .Set<Account>()
                .Project()
                .To<AccountView>()
                .OrderByDescending(account => account.CreationDate)
                .GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.Equal(expected.Current.RoleTitle, actual.Current.RoleTitle);
                Assert.Equal(expected.Current.IsLocked, actual.Current.IsLocked);
                Assert.Equal(expected.Current.Username, actual.Current.Username);
                Assert.Equal(expected.Current.Email, actual.Current.Email);
                Assert.Equal(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: IsLoggedIn(IPrincipal user)

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsLoggedIn_ReturnsUserAuthenticationState(Boolean expected)
        {
            IPrincipal user = Substitute.For<IPrincipal>();
            user.Identity.IsAuthenticated.Returns(expected);

            Boolean actual = service.IsLoggedIn(user);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: IsActive(String id)

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void IsActive_ReturnsAccountState(Boolean isLocked, Boolean expected)
        {
            account.IsLocked = isLocked;
            context.SaveChanges();

            Boolean actual = service.IsActive(account.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsActive_IsNotActiveThenAccountDoesNotExist()
        {
            Assert.False(service.IsActive("Test"));
        }

        #endregion

        #region Method: Recover(AccountRecoveryView view)

        [Fact]
        public void Recover_OnNonExistingAccountReturnsNullToken()
        {
            AccountRecoveryView view = ObjectFactory.CreateAccountRecoveryView();
            view.Email = "not@existing.email";

            Assert.Null(service.Recover(view));
        }

        [Fact]
        public void Recover_UpdatesAccountRecoveryInformation()
        {
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);

            AccountRecoveryView view = ObjectFactory.CreateAccountRecoveryView();
            view.Email = view.Email.ToUpper();

            String expectedToken = service.Recover(view);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.InRange(actual.RecoveryTokenExpirationDate.Value.Ticks,
                expected.RecoveryTokenExpirationDate.Value.Ticks - TimeSpan.TicksPerSecond,
                expected.RecoveryTokenExpirationDate.Value.Ticks + TimeSpan.TicksPerSecond);
            Assert.NotEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expectedToken, actual.RecoveryToken);
            Assert.Equal(expected.IsLocked, actual.IsLocked);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
            Assert.NotNull(actual.RecoveryToken);
        }

        #endregion

        #region Method: Register(AccountRegisterView view)

        [Fact]
        public void Register_RegistersAccount()
        {
            AccountRegisterView view = ObjectFactory.CreateAccountRegisterView(2);
            view.Email = view.Email.ToUpper();

            service.Register(view);

            Account actual = context.Set<Account>().AsNoTracking().Single(account => account.Id == view.Id);
            AccountRegisterView expected = view;

            Assert.Equal(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Email.ToLower(), actual.Email);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Null(actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.RecoveryToken);
            Assert.False(actual.IsLocked);
            Assert.Null(actual.RoleId);
            Assert.Null(actual.Role);
        }

        #endregion

        #region Method: Reset(AccountResetView view)

        [Fact]
        public void Reset_ResetsAccount()
        {
            Account account = context.Set<Account>().AsNoTracking().Single();
            AccountResetView view = ObjectFactory.CreateAccountResetView();
            hasher.HashPassword(view.NewPassword).Returns("Reset");
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;
            account.Passhash = "Reset";

            service.Reset(view);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.IsLocked, actual.IsLocked);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Create(AccountCreateView view)

        [Fact]
        public void Create_CreatesAccount()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);
            view.Email = view.Email.ToUpper();
            view.RoleId = account.RoleId;

            service.Create(view);

            Account actual = context.Set<Account>().AsNoTracking().Single(acc => acc.Id == view.Id);
            AccountCreateView expected = view;

            Assert.Equal(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Email.ToLower(), actual.Email);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Null(actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.RecoveryToken);
            Assert.False(actual.IsLocked);
        }

        [Fact]
        public void Create_RefreshesAuthorizationProvider()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);

            service.Create(view);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Edit(AccountEditView view)

        [Fact]
        public void Edit_EditsAccount()
        {
            Account account = context.Set<Account>().AsNoTracking().Single();
            AccountEditView view = ObjectFactory.CreateAccountEditView();
            view.IsLocked = account.IsLocked = !account.IsLocked;
            view.Username = account.Username + "Test";
            view.RoleId = account.RoleId = null;
            view.Email = account.Email + "s";

            service.Edit(view);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.IsLocked, actual.IsLocked);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_RefreshesAuthorizationProvider()
        {
            AccountEditView view = ObjectFactory.CreateAccountEditView();

            service.Edit(view);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Edit(ProfileEditView view)

        [Fact]
        public void Edit_EditsProfile()
        {
            Account account = context.Set<Account>().AsNoTracking().Single();
            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            account.Passhash = hasher.HashPassword(view.NewPassword);
            view.Username = account.Username += "Test";
            view.Email = account.Email += "Test";

            service.Edit(view);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Email.ToLower(), actual.Email);
            Assert.Equal(expected.IsLocked, actual.IsLocked);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Edit_OnNotSpecifiedNewPasswordDoesNotEditPassword(String newPassword)
        {
            String passhash = context.Set<Account>().AsNoTracking().Single().Passhash;
            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            view.NewPassword = newPassword;

            service.Edit(view);

            String actual = context.Set<Account>().AsNoTracking().Single().Passhash;
            String expected = passhash;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Fact]
        public void Delete_DeletesAccount()
        {
            service.Delete(account.Id);

            Assert.Empty(context.Set<Account>());
        }

        [Fact]
        public void Delete_RefreshesAuthorizationProvider()
        {
            service.Delete(account.Id);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Login(String username)

        [Fact]
        public void Login_IsCaseInsensitive()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(view.Username.ToUpper());

            String actual = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value).Name;
            String expected = view.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Login_CreatesPersistentAuthenticationTicket()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(view.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.True(ticket.IsPersistent);
        }

        [Fact]
        public void Login_SetAccountIdAsAuthenticationTicketValue()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(view.Username);

            String actual = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value).Name;
            String expected = view.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Fact]
        public void Logout_MakesAccountCookieExpired()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(view.Username);
            service.Logout();

            DateTime expirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.True(expirationDate < DateTime.Now);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}