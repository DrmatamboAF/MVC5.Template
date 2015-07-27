using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Data.Migrations
{
    public class InitialDataTests : IDisposable
    {
        private Context context;

        public InitialDataTests()
        {
            context = new Context();
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region Table: Roles

        [Fact]
        public void RolesTable_HasSysAdminRole()
        {
            Assert.NotNull(context.Set<Role>().SingleOrDefault(role => role.Title == "Sys_Admin"));
        }

        #endregion

        #region Table: Accounts

        [Fact]
        public void AccountsTable_HasSysAdminAccountWithAdminRole()
        {
            Assert.NotNull(context.Set<Account>()
                .SingleOrDefault(account =>
                    account.Username == "admin" &&
                    account.Role.Title == "Sys_Admin"));
        }

        #endregion

        #region Table: Privileges

        [Theory]
        [InlineData("Administration", "Accounts", "Index")]
        [InlineData("Administration", "Accounts", "Create")]
        [InlineData("Administration", "Accounts", "Details")]
        [InlineData("Administration", "Accounts", "Edit")]

        [InlineData("Administration", "Roles", "Index")]
        [InlineData("Administration", "Roles", "Create")]
        [InlineData("Administration", "Roles", "Details")]
        [InlineData("Administration", "Roles", "Edit")]
        [InlineData("Administration", "Roles", "Delete")]
        public void PrivilegesTable_HasPrivilege(String area, String controller, String action)
        {
            Assert.NotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Controller == controller &&
                privilege.Action == action &&
                privilege.Area == area));
        }

        [Fact]
        public void PrivilegesTable_HasExactNumberOfPrivileges()
        {
            Int32 actual = context.Set<Privilege>().Count();
            Int32 expected = 9;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Table: RolePrivileges

        [Fact]
        public void RolesPrivilegesTable_HasAllPrivilegesForAdminRole()
        {
            IEnumerable<String> expected = context
                .Set<Privilege>()
                .Select(privilege => privilege.Id)
                .OrderBy(privilegeId => privilegeId);
            IEnumerable<String> actual = context
                .Set<Role>()
                .Single(role => role.Title == "Sys_Admin")
                .RolePrivileges
                .Select(rolePrivilege => rolePrivilege.PrivilegeId)
                .OrderBy(privilegeId => privilegeId);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
