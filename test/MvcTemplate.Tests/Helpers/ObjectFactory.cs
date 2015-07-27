using MvcTemplate.Objects;
using MvcTemplate.Tests.Objects;
using System;

namespace MvcTemplate.Tests
{
    public static class ObjectFactory
    {
        #region Administration

        public static Account CreateAccount(Int32 id = 1)
        {
            return new Account
            {
                Id = "Id" + id,
                IsLocked = true,
                Username = "Username" + id,
                Passhash = "$2a$04$zNgYw403HgH1N69j4kj/peGI7SUvGiR5awIPZ2Yh/6O5BwyUO3qZe", // Password1
                Email = id + "@tests.com",

                RecoveryToken = "Token" + id,
                RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(5)
            };
        }

        public static ProfileEditView CreateProfileEditView(Int32 id = 1)
        {
            return new ProfileEditView
            {
                Id = "Id" + id,
                Email = id + "@tests.com",
                Username = "Username" + id,
                NewPassword = "NewPassword1",
                Password = "Password1"
            };
        }
        public static ProfileDeleteView CreateProfileDeleteView(Int32 id = 1)
        {
            return new ProfileDeleteView
            {
                Id = "Id" + id,
                Password = "Password1"
            };
        }

        public static AccountRegisterView CreateAccountRegisterView(Int32 id = 1)
        {
            return new AccountRegisterView
            {
                Id = "Id" + id,
                Username = "Username" + id,
                Email = id + "@tests.com",
                Password = "Password1"
            };
        }
        public static AccountRecoveryView CreateAccountRecoveryView(Int32 id = 1)
        {
            return new AccountRecoveryView
            {
                Id = "Id" + id,
                Email = id + "@tests.com"
            };
        }
        public static AccountCreateView CreateAccountCreateView(Int32 id = 1)
        {
            return new AccountCreateView
            {
                Id = "Id" + id,
                Username = "Username" + id,
                Email = id + "@tests.com",
                Password = "Password1"
            };
        }
        public static AccountResetView CreateAccountResetView(Int32 id = 1)
        {
            return new AccountResetView
            {
                Id = "Id" + id,

                Token = "Token" + id,
                NewPassword = "NewPassword1"
            };
        }
        public static AccountLoginView CreateAccountLoginView(Int32 id = 1)
        {
            return new AccountLoginView
            {
                Id = "Id" + id,
                Username = "Username" + id,
                Password = "Password1"
            };
        }
        public static AccountEditView CreateAccountEditView(Int32 id = 1)
        {
            return new AccountEditView
            {
                Id = "Id" + id,
                IsLocked = true,
                Username = "Username" + id
            };
        }

        public static Role CreateRole(Int32 id = 1)
        {
            return new Role
            {
                Id = "Id" + id,
                Title = "Title" + id
            };
        }
        public static RoleView CreateRoleView(Int32 id = 1)
        {
            return new RoleView
            {
                Id = "Id" + id,
                Title = "Title" + id
            };
        }

        public static Privilege CreatePrivilege(Int32 id = 1)
        {
            return new Privilege
            {
                Id = "Id" + id,
                Area = "Area" + id,
                Action = "Action" + id,
                Controller = "Controller" + id
            };
        }
        public static RolePrivilege CreateRolePrivilege(Int32 id = 1)
        {
            return new RolePrivilege
            {
                Id = "Id" + id
            };
        }

        #endregion

        #region Tests

        public static TestModel CreateTestModel(Int32 id = 1)
        {
            return new TestModel
            {
                Id = "Id" + id,
                Text = "Text" + id
            };
        }
        public static TestView CreateTestView(Int32 id = 1)
        {
            return new TestView
            {
                Id = "Id" + id,
                Text = "Text" + id
            };
        }

        #endregion
    }
}
