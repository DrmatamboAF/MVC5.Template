using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class MvcSiteMapParser : IMvcSiteMapParser
    {
        public IEnumerable<MvcSiteMapNode> GetNodeTree(XElement siteMap)
        {
            return GetNodes(siteMap, null);
        }

        private IEnumerable<MvcSiteMapNode> GetNodes(XElement siteMap, MvcSiteMapNode parent)
        {
            List<MvcSiteMapNode> nodes = new List<MvcSiteMapNode>();
            foreach (XElement siteMapNode in siteMap.Elements("siteMapNode"))
            {
                MvcSiteMapNode node = new MvcSiteMapNode();

                node.IsMenu = (Boolean?)siteMapNode.Attribute("menu") == true;
                node.Controller = (String)siteMapNode.Attribute("controller");
                node.IconClass = (String)siteMapNode.Attribute("icon");
                node.Action = (String)siteMapNode.Attribute("action");
                node.Area = (String)siteMapNode.Attribute("area");
                node.Children = GetNodes(siteMapNode, node);
                node.Parent = parent;

                nodes.Add(node);
            }

            return nodes;
        }
    }
}
