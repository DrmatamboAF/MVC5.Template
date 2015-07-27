using MvcTemplate.Components.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Security
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private Type[] ControllerTypes { get; set; }
        private Dictionary<String, IEnumerable<Privilege>> Cache { get; set; }

        public AuthorizationProvider(Assembly controllersAssembly)
        {
            ControllerTypes = controllersAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).ToArray();
        }

        public virtual Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            Type authorizedController = GetControllerType(area, controller);
            MethodInfo actionInfo = GetMethod(authorizedController, action);
            String authorizedAs = GetAuthorizedAs(actionInfo);
            if (String.IsNullOrEmpty(area)) area = null;

            if (authorizedAs != null)
                return IsAuthorizedFor(accountId, area, controller, authorizedAs);

            if (AllowsUnauthorized(authorizedController, actionInfo))
                return true;

            if (!Cache.ContainsKey(accountId ?? ""))
                return false;

            return Cache[accountId]
                .Any(privilege =>
                    String.Equals(privilege.Area, area, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Action, action, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Controller, controller, StringComparison.OrdinalIgnoreCase));
        }

        public virtual void Refresh()
        {
            using (IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>())
            {
                Cache = unitOfWork
                    .Select<Account>()
                    .Where(account =>
                        !account.IsLocked &&
                        account.RoleId != null)
                    .Select(account => new
                    {
                        Id = account.Id,
                        Privileges = account
                            .Role
                            .RolePrivileges
                            .Select(rolePrivilege => rolePrivilege.Privilege)
                    })
                    .ToDictionary(
                        account => account.Id,
                        account => account.Privileges);
            }
        }

        private Boolean AllowsUnauthorized(Type authorizedControllerType, MethodInfo method)
        {
            if (method.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (method.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            while (authorizedControllerType != typeof(Controller))
            {
                if (authorizedControllerType.IsDefined(typeof(AuthorizeAttribute), false)) return false;
                if (authorizedControllerType.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
                if (authorizedControllerType.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

                authorizedControllerType = authorizedControllerType.BaseType;
            }

            return true;
        }
        private Type GetControllerType(String area, String controller)
        {
            String controllerType = controller + "Controller";
            IEnumerable<Type> controllers = ControllerTypes
                .Where(type => type.Name.Equals(controllerType, StringComparison.OrdinalIgnoreCase));

            if (String.IsNullOrEmpty(area))
                controllers = controllers.Where(type =>
                    !type.IsDefined(typeof(AreaAttribute), false));
            else
                controllers = controllers.Where(type =>
                    type.IsDefined(typeof(AreaAttribute), false) &&
                    String.Equals(type.GetCustomAttribute<AreaAttribute>(false).Name, area, StringComparison.OrdinalIgnoreCase));

            return controllers.Single();
        }
        private MethodInfo GetMethod(Type controller, String action)
        {
            MethodInfo[] methods = controller
                .GetMethods()
                .Where(method =>
                    (
                        !method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    (
                        method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.GetCustomAttribute<ActionNameAttribute>(false).Name.ToLowerInvariant() == action.ToLowerInvariant()
                    ))
                .ToArray();

            MethodInfo getMethod = methods.FirstOrDefault(method => method.IsDefined(typeof(HttpGetAttribute), false));
            if (getMethod != null)
                return getMethod;

            if (methods.Length == 0)
                throw new Exception(String.Format("'{0}' does not have '{1}' action.", controller.Name, action));

            return methods[0];
        }
        private String GetAuthorizedAs(MemberInfo action)
        {
            if (!action.IsDefined(typeof(AuthorizeAsAttribute), false))
                return null;

            AuthorizeAsAttribute authorizedAs = action.GetCustomAttribute<AuthorizeAsAttribute>(false);
            if (authorizedAs.Action == action.Name)
                return null;

            return authorizedAs.Action;
        }
    }
}
