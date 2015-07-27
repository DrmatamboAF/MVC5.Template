using MvcTemplate.Components.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MvcSiteMapParserTests
    {
        private static MvcSiteMapParser parser;
        private static XElement siteMap;

        static MvcSiteMapParserTests()
        {
            parser = new MvcSiteMapParser();
            siteMap = CreateSiteMap();
        }

        #region Method: GetNodeTree(XElement siteMap)

        [Fact]
        public void GetNodes_ReturnsAllSiteMapNodes()
        {
            IEnumerator<MvcSiteMapNode> actual = TreeToEnumerable(parser.GetNodeTree(siteMap)).GetEnumerator();
            IEnumerator<MvcSiteMapNode> expected = TreeToEnumerable(GetExpectedNodeTree()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Controller, actual.Current.Controller);
                Assert.Equal(expected.Current.IconClass, actual.Current.IconClass);
                Assert.Equal(expected.Current.IsMenu, actual.Current.IsMenu);
                Assert.Equal(expected.Current.Action, actual.Current.Action);
                Assert.Equal(expected.Current.Area, actual.Current.Area);

                if (expected.Current.Parent != null || actual.Current.Parent != null)
                {
                    Assert.Equal(expected.Current.Parent.Controller, actual.Current.Parent.Controller);
                    Assert.Equal(expected.Current.Parent.IconClass, actual.Current.Parent.IconClass);
                    Assert.Equal(expected.Current.Parent.IsMenu, actual.Current.Parent.IsMenu);
                    Assert.Equal(expected.Current.Parent.Action, actual.Current.Parent.Action);
                    Assert.Equal(expected.Current.Parent.Area, actual.Current.Parent.Area);
                }
            }
        }

        #endregion

        #region Test helpers

        private static XElement CreateSiteMap()
        {
            return XElement.Parse(
            @"<siteMap>
              <siteMapNode controller=""Home"" action=""Index"" menu=""false"" icon=""fa fa-home"">
                <siteMapNode menu=""true"" area=""Administration"" icon=""fa fa-gears"">
                  <siteMapNode controller=""Accounts"" action=""Index"" menu=""false"" area=""Administration"" icon=""fa fa-user"">
                    <siteMapNode controller=""Accounts"" action=""Create"" menu=""true"" area=""Administration"" icon=""fa fa-file-o"" />
                  </siteMapNode>
                </siteMapNode>
              </siteMapNode>
            </siteMap>");
        }
        private IEnumerable<MvcSiteMapNode> GetExpectedNodeTree()
        {
            MvcSiteMapNode[] map =
            {
                new MvcSiteMapNode
                {
                    IconClass = "fa fa-home",

                    Controller = "Home",
                    Action = "Index",

                    Children = new List<MvcSiteMapNode>
                    {
                        new MvcSiteMapNode
                        {
                            IconClass = "fa fa-gears",
                            IsMenu = true,

                            Area = "Administration",

                            Children = new List<MvcSiteMapNode>
                            {
                                new MvcSiteMapNode
                                {
                                    IconClass = "fa fa-user",
                                    IsMenu = false,

                                    Area = "Administration",
                                    Controller = "Accounts",
                                    Action = "Index",

                                    Children = new List<MvcSiteMapNode>
                                    {
                                        new MvcSiteMapNode
                                        {
                                            IconClass = "fa fa-file-o",
                                            IsMenu = true,

                                            Area = "Administration",
                                            Controller = "Accounts",
                                            Action = "Create",

                                            Children = new List<MvcSiteMapNode>()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            foreach (MvcSiteMapNode level1Node in map)
            {
                foreach (MvcSiteMapNode level2Node in level1Node.Children)
                {
                    level2Node.Parent = level1Node;

                    foreach (MvcSiteMapNode level3Node in level2Node.Children)
                    {
                        level3Node.Parent = level2Node;

                        foreach (MvcSiteMapNode level4Node in level3Node.Children)
                        {
                            level4Node.Parent = level3Node;
                        }
                    }
                }
            }

            return map;
        }

        private IEnumerable<MvcSiteMapNode> TreeToEnumerable(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> list = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToEnumerable(node.Children));
            }

            return list;
        }

        #endregion
    }
}
