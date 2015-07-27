using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Security;

namespace MvcTemplate.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IHasher Hasher { get; set; }

        public AccountService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            Hasher = hasher;
        }

        public TView Get<TView>(String id) where TView : BaseView
        {
            return UnitOfWork.GetAs<Account, TView>(id);
        }
        public IQueryable<AccountView> GetViews()
        {
            return UnitOfWork
                .Select<Account>()
                .To<AccountView>()
                .OrderByDescending(account => account.CreationDate);
        }

        public Boolean IsLoggedIn(IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
        public Boolean IsActive(String id)
        {
            return UnitOfWork.Select<Account>().Any(account => account.Id == id && !account.IsLocked);
        }

        public String Recover(AccountRecoveryView view)
        {
            Account account = UnitOfWork.Select<Account>().SingleOrDefault(acc => acc.Email.ToLower() == view.Email.ToLower());
            if (account == null) return null;

            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
            account.RecoveryToken = Guid.NewGuid().ToString();

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            return account.RecoveryToken;
        }
        public void Register(AccountRegisterView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            account.Passhash = Hasher.HashPassword(view.Password);
            account.Email = view.Email.ToLower();

            UnitOfWork.Insert(account);
            UnitOfWork.Commit();
        }
        public void Reset(AccountResetView view)
        {
            Account account = UnitOfWork.Select<Account>().Single(acc => acc.RecoveryToken == view.Token);
            account.Passhash = Hasher.HashPassword(view.NewPassword);
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }

        public void Create(AccountCreateView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            account.Passhash = Hasher.HashPassword(view.Password);
            account.Email = view.Email.ToLower();

            UnitOfWork.Insert(account);
            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }
        public void Edit(AccountEditView view)
        {
            Account account = UnitOfWork.Get<Account>(view.Id);
            account.IsLocked = view.IsLocked;
            account.RoleId = view.RoleId;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }

        public void Edit(ProfileEditView view)
        {
            Account account = UnitOfWork.Get<Account>(view.Id);
            account.Email = view.Email.ToLower();
            account.Username = view.Username;

            if (!String.IsNullOrWhiteSpace(view.NewPassword))
                account.Passhash = Hasher.HashPassword(view.NewPassword);

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }
        public void Delete(String id)
        {
            UnitOfWork.Delete<Account>(id);
            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }

        public void Login(String username)
        {
            FormsAuthentication.SetAuthCookie(GetAccountId(username), true);
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private String GetAccountId(String username)
        {
            return UnitOfWork
                .Select<Account>()
                .Where(account => account.Username.ToLower() == username.ToLower())
                .Select(account => account.Id)
                .Single();
        }
    }
}
