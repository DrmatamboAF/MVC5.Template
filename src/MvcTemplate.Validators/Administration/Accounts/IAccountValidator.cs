using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Validators
{
    public interface IAccountValidator : IValidator
    {
        Boolean CanRegister(AccountRegisterView view);
        Boolean CanRecover(AccountRecoveryView view);
        Boolean CanReset(AccountResetView view);
        Boolean CanLogin(AccountLoginView view);

        Boolean CanCreate(AccountCreateView view);
        Boolean CanEdit(AccountEditView view);

        Boolean CanEdit(ProfileEditView view);
        Boolean CanDelete(ProfileDeleteView view);
    }
}
