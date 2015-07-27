using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace MvcTemplate.Resources
{
    public static class ResourceProvider
    {
        private static Dictionary<String, ResourceManager> Resources { get; set; }

        static ResourceProvider()
        {
            Resources = new Dictionary<String, ResourceManager>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                PropertyInfo property = type.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.Static);
                if (property != null)
                {
                    ResourceManager manager = property.GetValue(null) as ResourceManager;
                    manager.IgnoreCase = true;

                    Resources.Add(type.FullName, manager);
                }
            }
        }

        public static String GetDatalistTitle(String datalist)
        {
            return GetResource("MvcTemplate.Resources.Datalist.Titles", datalist ?? "");
        }
        public static String GetContentTitle(RouteValueDictionary values)
        {
            String area = values["area"] as String;
            String action = values["action"] as String;
            String controller = values["controller"] as String;

            return GetResource("MvcTemplate.Resources.Shared.ContentTitles", area + controller + action);
        }
        public static String GetSiteMapTitle(String area, String controller, String action)
        {
            return GetResource("MvcTemplate.Resources.SiteMap.Titles", area + controller + action);
        }

        public static String GetPrivilegeAreaTitle(String area)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Area.Titles", area ?? "");
        }
        public static String GetPrivilegeControllerTitle(String area, String controller)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Controller.Titles", area + controller);
        }
        public static String GetPrivilegeActionTitle(String area, String controller, String action)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Action.Titles", area + controller + action);
        }

        public static String GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression expression = property.Body as MemberExpression;
            if (expression == null) return null;

            return GetPropertyTitle(typeof(TModel), expression.Member.Name);
        }
        public static String GetPropertyTitle(Type view, String property)
        {
            return GetPropertyTitle(view.Name, property ?? "");
        }

        private static String GetPropertyTitle(String view, String property)
        {
            String baseName = "MvcTemplate.Resources.Views." + view + ".Titles";
            String title = GetResource(baseName, property);
            if (title != null) return title;

            String[] camelCasedProperties = SplitCamelCase(property);
            for (Int32 skippedProperties = 0; skippedProperties < camelCasedProperties.Length; skippedProperties++)
            {
                for (Int32 viewSize = 1; viewSize < camelCasedProperties.Length - skippedProperties; viewSize++)
                {
                    String joinedView = String.Concat(camelCasedProperties.Skip(skippedProperties).Take(viewSize)) + "View";
                    String joinedProperty = String.Concat(camelCasedProperties.Skip(viewSize + skippedProperties));
                    String joinedBaseName = "MvcTemplate.Resources.Views." + joinedView + ".Titles";

                    title = GetResource(joinedBaseName, joinedProperty);
                    if (title != null) return title;
                }
            }

            return null;
        }
        private static String GetResource(String type, String key)
        {
            if (!Resources.ContainsKey(type)) return null;

            return Resources[type].GetString(key);
        }
        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, @"(?<!^)(?=[A-Z])");
        }
    }
}
