using MvcTemplate.Components.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using Xunit;

namespace MvcTemplate.Tests.Unit.Resources
{
    public class ResourcesTests
    {
        [Fact]
        public void Resources_HasAllPrivilegeAreaTitles()
        {
            ResourceManager manager = MvcTemplate.Resources.Privilege.Area.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] areas = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area)
                    .Distinct()
                    .ToArray();

                foreach (String area in areas)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(area)),
                        String.Format("Privilege area '{0}', does not have a title.", area));
            }
        }

        [Fact]
        public void Resources_HasAllPrivilegeControllerTitles()
        {
            ResourceManager manager = MvcTemplate.Resources.Privilege.Controller.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] controllers = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area + priv.Controller)
                    .Distinct()
                    .ToArray();

                foreach (String controller in controllers)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(controller)),
                        String.Format("Privilege controller '{0}', does not have a title.", controller));
            }
        }

        [Fact]
        public void Resources_HasAllPrivilegeActionTitles()
        {
            ResourceManager manager = MvcTemplate.Resources.Privilege.Action.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] actions = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area + priv.Controller + priv.Action)
                    .Distinct()
                    .ToArray();

                foreach (String action in actions)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(action)),
                        String.Format("Privilege action '{0}', does not have a title.", action));
            }
        }

        [Fact]
        public void Resources_HasAllTranslations()
        {
            IEnumerable<Language> languages = GlobalizationProviderFactory.CreateProvider().Languages;
            IEnumerable<Type> resourceTypes = Assembly
                .Load("MvcTemplate.Resources")
                .GetTypes()
                .Where(type => type.Namespace.StartsWith("MvcTemplate.Resources."));

            foreach (Type type in resourceTypes)
            {
                ResourceManager manager = new ResourceManager(type);
                IEnumerable<String> resourceKeys = new String[0];

                foreach (ResourceSet set in languages.Select(language => manager.GetResourceSet(language.Culture, true, true)))
                {
                    resourceKeys = resourceKeys.Union(set.Cast<DictionaryEntry>().Select(resource => resource.Key.ToString()));
                    resourceKeys = resourceKeys.Distinct();
                }

                foreach (Language language in languages)
                {
                    ResourceSet set = manager.GetResourceSet(language.Culture, true, true);
                    foreach (String key in resourceKeys)
                        Assert.True(!String.IsNullOrEmpty(set.GetString(key)),
                            String.Format("{0}, does not have translation for '{1}' in {2} language.",
                                type.FullName, key, language.Culture.EnglishName));
                }
            }
        }
    }
}
