using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Extensions.Html
{
    public class JsTreeNode
    {
        public String Id { get; set; }
        public String Title { get; set; }

        public IList<JsTreeNode> Nodes { get; set; }

        public JsTreeNode(String id, String title)
        {
            Id = id;
            Title = title;
            Nodes = new List<JsTreeNode>();
        }
        public JsTreeNode(String title)
            : this(null, title)
        {
        }
    }
}