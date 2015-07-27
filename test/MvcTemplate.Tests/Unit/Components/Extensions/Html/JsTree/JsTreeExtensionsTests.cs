using MvcTemplate.Components.Extensions.Html;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeViewExtensionsTests
    {
        #region Extension method: JsTreeFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> expression)

        [Fact]
        public void JsTreeFor_FormsJsTreeFor()
        {
            JsTreeView tree = new JsTreeView();
            tree.JsTree.SelectedIds.Add("12345");
            tree.JsTree.Nodes.Add(new JsTreeNode("Test"));
            tree.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("12345", "Test1"));
            tree.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("23456", "Test2"));
            HtmlHelper<JsTreeView> html = HtmlHelperFactory.CreateHtmlHelper(tree);

            String actual = html.JsTreeFor(model => model.JsTree).ToString();
            String expected =
                "<div class=\"js-tree-view-ids\">" +
                    "<input name=\"JsTree.SelectedIds\" type=\"hidden\" value=\"12345\" />" +
                "</div>" +
                "<div class=\"js-tree-view\" for=\"JsTree.SelectedIds\">" +
                    "<ul>" +
                        "<li>Test" +
                            "<ul>" +
                                "<li id=\"12345\">Test1</li>" +
                                "<li id=\"23456\">Test2</li>" +
                            "</ul>" +
                        "</li>" +
                    "</ul>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
