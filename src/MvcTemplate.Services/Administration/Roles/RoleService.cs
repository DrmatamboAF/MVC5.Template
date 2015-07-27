using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Privilege;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual void SeedPrivilegesTree(RoleView view)
        {
            JsTreeNode rootNode = new JsTreeNode(Titles.All);
            view.PrivilegesTree.Nodes.Add(rootNode);

            IEnumerable<Privilege> privileges = GetAllPrivileges();
            foreach (IGrouping<String, Privilege> area in privileges.GroupBy(privilege => privilege.Area))
            {
                JsTreeNode areaNode = new JsTreeNode(area.Key);
                foreach (IGrouping<String, Privilege> controller in area.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controller.Key);
                    foreach (IGrouping<String, Privilege> action in controller.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new JsTreeNode(action.First().Id, action.Key));

                    if (areaNode.Title == null)
                        rootNode.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Title != null)
                    rootNode.Nodes.Add(areaNode);
            }
        }
        private IEnumerable<Privilege> GetAllPrivileges()
        {
            return UnitOfWork
                .Select<Privilege>()
                .ToList()
                .Select(privilege => new Privilege
                {
                    Id = privilege.Id,
                    Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                    Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Area, privilege.Controller),
                    Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Area, privilege.Controller, privilege.Action)
                })
                .OrderBy(privilege => privilege.Area ?? privilege.Controller);
        }

        public IQueryable<RoleView> GetViews()
        {
            return UnitOfWork
                .Select<Role>()
                .To<RoleView>()
                .OrderByDescending(role => role.CreationDate);
        }
        public RoleView GetView(String id)
        {
            RoleView role = UnitOfWork.GetAs<Role, RoleView>(id);
            if (role != null)
            {
                role.PrivilegesTree.SelectedIds = UnitOfWork
                    .Select<RolePrivilege>()
                    .Where(rolePrivilege => rolePrivilege.RoleId == role.Id)
                    .Select(rolePrivilege => rolePrivilege.PrivilegeId)
                    .ToList();

                SeedPrivilegesTree(role);
            }

            return role;
        }

        public void Create(RoleView view)
        {
            CreateRole(view);
            CreateRolePrivileges(view);

            UnitOfWork.Commit();
        }
        public void Edit(RoleView view)
        {
            DeleteRolePrivileges(view.Id);
            CreateRolePrivileges(view);
            EditRole(view);

            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }
        public void Delete(String id)
        {
            RemoveRoleFromAccounts(id);
            DeleteRolePrivileges(id);
            DeleteRole(id);

            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }

        private void CreateRole(RoleView view)
        {
            Role role = UnitOfWork.To<Role>(view);

            UnitOfWork.Insert(role);
        }
        private void EditRole(RoleView view)
        {
            Role role = UnitOfWork.To<Role>(view);

            UnitOfWork.Update(role);
        }
        private void DeleteRole(String id)
        {
            UnitOfWork.Delete<Role>(id);
        }

        private void DeleteRolePrivileges(String roleId)
        {
            IQueryable<RolePrivilege> rolePrivileges = UnitOfWork
                .Select<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == roleId);

            foreach (RolePrivilege rolePrivilege in rolePrivileges)
                UnitOfWork.Delete(rolePrivilege);
        }
        private void CreateRolePrivileges(RoleView role)
        {
            foreach (String privilegeId in role.PrivilegesTree.SelectedIds)
                UnitOfWork.Insert(new RolePrivilege
                {
                    RoleId = role.Id,
                    PrivilegeId = privilegeId
                });
        }
        private void RemoveRoleFromAccounts(String roleId)
        {
            IQueryable<Account> accountsWithRole = UnitOfWork
                .Select<Account>()
                .Where(account => account.RoleId == roleId);

            foreach (Account account in accountsWithRole)
            {
                account.RoleId = null;
                UnitOfWork.Update(account);
            }
        }
    }
}
