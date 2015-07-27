using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MvcTemplate.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<Context>, IDisposable
    {
        private IUnitOfWork UnitOfWork { get; set; }

        public Configuration()
        {
            ContextKey = "Context";
        }

        protected override void Seed(Context context)
        {
            IAuditLogger logger = new AuditLogger(new Context(), "sys_seeder");
            UnitOfWork = new UnitOfWork(context, logger);

            SeedPrivileges();
            SeedRoles();

            SeedAccounts();
        }

        #region Administration

        private void SeedPrivileges()
        {
            Privilege[] privileges =
            {
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Index" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Create" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Details" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Edit" },

                new Privilege { Area = "Administration", Controller = "Roles", Action = "Index" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Create" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Details" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Edit" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Delete" }
            };

            DeleteUnused(privileges);
            CreateMissing(privileges);
        }
        private void DeleteUnused(Privilege[] privileges)
        {
            foreach (Privilege privilege in UnitOfWork.Select<Privilege>())
                if (!privileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    foreach (RolePrivilege rolePrivilege in UnitOfWork.Select<RolePrivilege>().Where(rolePriv => rolePriv.PrivilegeId == privilege.Id))
                        UnitOfWork.Delete(rolePrivilege);

                    UnitOfWork.Delete(privilege);
                }

            UnitOfWork.Commit();
        }
        private void CreateMissing(Privilege[] privileges)
        {
            Privilege[] dbPrivileges = UnitOfWork.Select<Privilege>().ToArray();
            foreach (Privilege privilege in privileges)
                if (!dbPrivileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    UnitOfWork.Insert(privilege);
                }

            UnitOfWork.Commit();
        }

        private void SeedRoles()
        {
            if (!UnitOfWork.Select<Role>().Any(role => role.Title == "Sys_Admin"))
            {
                UnitOfWork.Insert(new Role { Title = "Sys_Admin" });
                UnitOfWork.Commit();
            }

            String adminRoleId = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id;
            RolePrivilege[] adminPrivileges = UnitOfWork
                .Select<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToArray();

            foreach (Privilege privilege in UnitOfWork.Select<Privilege>())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    UnitOfWork.Insert(new RolePrivilege
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            UnitOfWork.Commit();
        }

        private void SeedAccounts()
        {
            Account[] accounts =
            {
                new Account
                {
                    Username = "admin",
                    Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq", // Admin123?
                    Email = "admin@admins.com",
                    IsLocked = false,

                    RoleId = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id
                }
            };

            foreach (Account account in accounts)
            {
                Account dbAccount = UnitOfWork.Select<Account>().FirstOrDefault(acc => acc.Username == account.Username);
                if (dbAccount != null)
                {
                    dbAccount.IsLocked = account.IsLocked;

                    UnitOfWork.Update(dbAccount);
                }
                else
                {
                    UnitOfWork.Insert(account);
                }
            }

            UnitOfWork.Commit();
        }

        #endregion

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
