using MvcTemplate.Components.Extensions.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.RoleView;
using System;
using System.Linq;

namespace MvcTemplate.Validators
{
    public class RoleValidator : BaseValidator, IRoleValidator
    {
        public RoleValidator(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Boolean CanCreate(RoleView view)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueRole(view);

            return isValid;
        }
        public Boolean CanEdit(RoleView view)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueRole(view);

            return isValid;
        }

        private Boolean IsUniqueRole(RoleView view)
        {
            Boolean isUnique = !UnitOfWork
                .Select<Role>()
                .Any(role =>
                    role.Id != view.Id &&
                    role.Title.ToLower() == view.Title.ToLower());

            if (!isUnique)
                ModelState.AddModelError<RoleView>(role => role.Title, Validations.TitleIsAlreadyUsed);

            return isUnique;
        }
    }
}
