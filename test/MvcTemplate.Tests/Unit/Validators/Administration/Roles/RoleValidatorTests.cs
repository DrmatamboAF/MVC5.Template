using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.RoleView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Validators
{
    public class RoleValidatorTests : IDisposable
    {
        private RoleValidator validator;
        private TestingContext context;
        private Role role;

        public RoleValidatorTests()
        {
            context = new TestingContext();
            validator = new RoleValidator(new UnitOfWork(context));

            TearDownData();
            SetUpData();
        }
        public void Dispose()
        {
            context.Dispose();
            validator.Dispose();
        }

        #region Method: CanCreate(RoleView view)

        [Fact]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        [Fact]
        public void CanCreate_CanNotCreateWithAlreadyUsedRoleTitle()
        {
            RoleView view = ObjectFactory.CreateRoleView();
            view.Title = role.Title.ToLower();
            view.Id += "Test";

            Assert.False(validator.CanCreate(view));
        }

        [Fact]
        public void CanCreate_AddsErrorMessageThenCanNotCreateWithAlreadyUsedRoleTitle()
        {
            RoleView view = ObjectFactory.CreateRoleView();
            view.Title = role.Title.ToLower();
            view.Id += "Test";

            validator.CanCreate(view);

            String actual = validator.ModelState["Title"].Errors[0].ErrorMessage;
            String expected = Validations.TitleIsAlreadyUsed;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreate_CanCreateValidRole()
        {
            Assert.True(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Method: CanEdit(RoleView view)

        [Fact]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateRoleView()));
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyUsedRoleTitle()
        {
            RoleView view = ObjectFactory.CreateRoleView();
            view.Title = role.Title.ToLower();
            view.Id += "Test";

            Assert.False(validator.CanEdit(view));
        }

        [Fact]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyUsedRoleTitle()
        {
            RoleView view = ObjectFactory.CreateRoleView();
            view.Title = role.Title.ToLower();
            view.Id += "Test";

            validator.CanEdit(view);

            String actual = validator.ModelState["Title"].Errors[0].ErrorMessage;
            String expected = Validations.TitleIsAlreadyUsed;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_CanEditValidRole()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();
            role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;

            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();

            Int32 privilegeNumber = 1;
            IEnumerable<String> controllers = new[] { "Accounts", "Roles" };
            IEnumerable<String> actions = new[] { "Index", "Create", "Details", "Edit", "Delete" };

            foreach (String controller in controllers)
                foreach (String action in actions)
                {
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(privilegeNumber++);
                    rolePrivilege.Privilege = new Privilege { Area = "Administration", Controller = controller, Action = action };
                    rolePrivilege.Privilege.Id = rolePrivilege.Id;
                    rolePrivilege.PrivilegeId = rolePrivilege.Id;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = role;

                    role.RolePrivileges.Add(rolePrivilege);
                }

            context.Set<Role>().Add(role);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
