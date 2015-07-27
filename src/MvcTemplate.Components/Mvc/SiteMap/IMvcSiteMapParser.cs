using System.Collections.Generic;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapParser
    {
        IEnumerable<MvcSiteMapNode> GetNodeTree(XElement siteMap);
    }
}
