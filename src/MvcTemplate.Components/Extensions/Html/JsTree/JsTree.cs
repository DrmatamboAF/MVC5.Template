using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Extensions.Html
{
    public class JsTree
    {
        public IList<JsTreeNode> Nodes { get; set; }
        public IList<String> SelectedIds { get; set; }

        public JsTree()
        {
            Nodes = new List<JsTreeNode>();
            SelectedIds = new List<String>();
        }
    }
}
