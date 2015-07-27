using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        TView Get<TView>(String id) where TView : BaseView;
        IQueryable<AccountView> GetViews();

        Boolean IsLoggedIn(IPrincipal user);
        Boolean IsActive(String id);

        String Recover(AccountRecoveryView view);
        void Register(AccountRegisterView view);
        void Reset(AccountResetView view);

        void Create(AccountCreateView view);
        void Edit(AccountEditView view);

        void Edit(ProfileEditView view);
        void Delete(String id);

        void Login(String username);
        void Logout();
    }
}
