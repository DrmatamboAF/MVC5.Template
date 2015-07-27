using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Privilege;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Services
{
    public class RoleServiceTests : IDisposable
    {
        private TestingContext context;
        private RoleService service;

        public RoleServiceTests()
        {
            context = new TestingContext();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            service = Substitute.ForPartsOf<RoleService>(new UnitOfWork(context));

            TearDownData();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
            context.Dispose();
            service.Dispose();
        }

        #region Method: SeedPrivilegesTree(RoleView view)

        [Fact]
        public void SeedPrivilegesTree_SeedsFirstLevelNodes()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_SeedsSecondLevelNodes()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_SeedsThirdLevelNodes()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_SeedsBranchesWithoutId()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(nodes);

            Assert.Empty(branches.Where(branch => branch.Id != null));
        }

        [Fact]
        public void SeedPrivilegesTree_SeedsLeafsWithId()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> leafs = GetAllLeafNodes(nodes);

            Assert.Empty(leafs.Where(leaf => leaf.Id == null));
        }

        #endregion

        #region Method: GetViews()

        [Fact]
        public void GetViews_GetsRoleViews()
        {
            context.Set<Role>().Add(ObjectFactory.CreateRole(1));
            context.Set<Role>().Add(ObjectFactory.CreateRole(2));
            context.SaveChanges();

            IEnumerator<RoleView> actual = service.GetViews().GetEnumerator();
            IEnumerator<RoleView> expected = context
                .Set<Role>()
                .Project()
                .To<RoleView>()
                .OrderByDescending(role => role.CreationDate)
                .GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.PrivilegesTree.SelectedIds, actual.Current.PrivilegesTree.SelectedIds);
                Assert.Equal(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: GetView(String id)

        [Fact]
        public void GetView_OnNotFoundRoleReturnsNull()
        {
            Assert.Null(service.GetView(""));
        }

        [Fact]
        public void GetView_GetsViewById()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView expected = Mapper.Map<RoleView>(role);
            RoleView actual = service.GetView(role.Id);

            Assert.Equal(expected.PrivilegesTree.SelectedIds, actual.PrivilegesTree.SelectedIds);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void GetView_SeedsSelectedIds()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = CreateRoleWithPrivileges();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            IEnumerable<String> expected = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId);
            IEnumerable<String> actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetView_SeedsPrivilegesTree()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = service.GetView(role.Id);

            service.Received().SeedPrivilegesTree(roleView);
        }

        #endregion

        #region Method: Create(RoleView view)

        [Fact]
        public void Create_CreatesRole()
        {
            RoleView view = ObjectFactory.CreateRoleView();

            service.Create(view);

            Role actual = context.Set<Role>().AsNoTracking().SingleOrDefault();
            RoleView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Create_CreatesRolePrivileges()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            service.Create(CreateRoleView());

            IEnumerable<String> expected = privileges.Select(privilege => privilege.Id).OrderBy(privilegeId => privilegeId);
            IEnumerable<String> actual = context
                .Set<Role>()
                .AsNoTracking()
                .Single()
                .RolePrivileges
                .Select(role => role.PrivilegeId)
                .OrderBy(privilegeId => privilegeId);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Fact]
        public void Edit_EditsRole()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.Title = role.Title += "Test";

            service.Edit(roleView);

            Role actual = context.Set<Role>().AsNoTracking().Single();
            RoleView expected = roleView;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_EditsRolePrivileges()
        {
            Role role = CreateRoleWithPrivileges();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = CreateRoleView();
            roleView.PrivilegesTree.SelectedIds.RemoveAt(0);

            service.Edit(roleView);

            IEnumerable<String> actual = context.Set<RolePrivilege>().AsNoTracking().Select(rolePriv => rolePriv.PrivilegeId);
            IEnumerable<String> expected = CreateRoleView().PrivilegesTree.SelectedIds.Skip(1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Edit_RefreshesAuthorizationProvider()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Edit(ObjectFactory.CreateRoleView());

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Fact]
        public void Delete_NullifiesDeletedRoleInAccounts()
        {
            Account account = ObjectFactory.CreateAccount();
            Role role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;
            account.Role = role;

            context.Set<Account>().Add(account);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.NotEmpty(context.Set<Account>().Where(acc => acc.Id == account.Id && acc.RoleId == null));
        }

        [Fact]
        public void Delete_DeletesRole()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.Empty(context.Set<Role>());
        }

        [Fact]
        public void Delete_DeletesRolePrivileges()
        {
            RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
            Privilege privilege = ObjectFactory.CreatePrivilege();
            Role role = ObjectFactory.CreateRole();

            rolePrivilege.PrivilegeId = privilege.Id;
            rolePrivilege.Privilege = privilege;
            rolePrivilege.RoleId = role.Id;
            rolePrivilege.Role = role;

            context.Set<RolePrivilege>().Add(rolePrivilege);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.Empty(context.Set<RolePrivilege>());
        }

        [Fact]
        public void Delete_RefreshesAuthorizationProvider()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Test helpers

        private JsTree CreatePrivilegesTree(Role role)
        {
            JsTreeNode rootNode = new JsTreeNode(Titles.All);
            JsTree expectedTree = new JsTree();

            expectedTree.Nodes.Add(rootNode);
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToList();

            IEnumerable<Privilege> allPrivileges = role
                .RolePrivileges
                .Select(rolePriv => rolePriv.Privilege)
                .Select(privilege => new Privilege
                {
                    Id = privilege.Id,
                    Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                    Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Area, privilege.Controller),
                    Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Area, privilege.Controller, privilege.Action)
                });

            foreach (IGrouping<String, Privilege> areaPrivilege in allPrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                JsTreeNode areaNode = new JsTreeNode(areaPrivilege.Key);
                foreach (IGrouping<String, Privilege> controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controllerPrivilege.Key);
                    foreach (IGrouping<String, Privilege> actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new JsTreeNode(actionPrivilege.First().Id, actionPrivilege.Key));

                    if (areaNode.Title == null)
                        rootNode.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Title != null)
                    rootNode.Nodes.Add(areaNode);
            }

            return expectedTree;
        }
        private Role CreateRoleWithPrivileges()
        {
            String[] actions = { "Edit", "Delete" };
            String[] controllers = { "Roles", "Profile" };

            Int32 privilegeNumber = 1;
            Role role = ObjectFactory.CreateRole();
            role.RolePrivileges = new List<RolePrivilege>();

            foreach (String controller in controllers)
                foreach (String action in actions)
                {
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(privilegeNumber++);
                    rolePrivilege.Privilege = new Privilege { Controller = controller, Action = action };
                    rolePrivilege.Privilege.Area = controller != "Roles" ? "Administration" : null;
                    rolePrivilege.Privilege.Id = rolePrivilege.Id;
                    rolePrivilege.PrivilegeId = rolePrivilege.Id;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = role;

                    role.RolePrivileges.Add(rolePrivilege);
                }

            return role;
        }
        private RoleView CreateRoleView()
        {
            Role role = CreateRoleWithPrivileges();
            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.PrivilegesTree = CreatePrivilegesTree(role);

            return roleView;
        }
        private void TearDownData()
        {
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        private IEnumerable<JsTreeNode> GetAllBranchNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0).ToList();
            foreach (JsTreeNode branch in branches.ToArray())
                branches.AddRange(GetAllBranchNodes(branch.Nodes));

            return branches;
        }
        private IEnumerable<JsTreeNode> GetAllLeafNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> leafs = nodes.Where(node => node.Nodes.Count == 0).ToList();
            IEnumerable<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0);

            foreach (JsTreeNode branch in branches)
                leafs.AddRange(GetAllLeafNodes(branch.Nodes));

            return leafs;
        }

        #endregion
    }
}
