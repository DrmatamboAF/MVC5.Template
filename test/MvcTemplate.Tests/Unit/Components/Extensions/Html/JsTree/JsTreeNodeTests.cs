using MvcTemplate.Components.Extensions.Html;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeNodeTests
    {
        #region Constructor: JsTreeNode(String title)

        [Fact]
        public void JsTreeNode_Title_SetsIdToNull()
        {
            Assert.Null(new JsTreeNode("Title").Id);
        }

        [Fact]
        public void JsTreeNode_Title_SetsTitle()
        {
            Assert.Equal("Title", new JsTreeNode("Title").Title);
        }

        [Fact]
        public void JsTreeNode_Title_CreatesEmptyTree()
        {
            Assert.Empty(new JsTreeNode("Title").Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String id, String title)

        [Fact]
        public void JsTreeNode_Id_Title_SetsId()
        {
            Assert.Equal("Id", new JsTreeNode("Id", null).Id);
        }

        [Fact]
        public void JsTreeNode_Id_Title_SetsTitle()
        {
            Assert.Equal("Title", new JsTreeNode(null, "Title").Title);
        }

        [Fact]
        public void JsTreeNode_Id_Title_CreatesEmptyTree()
        {
            Assert.Empty(new JsTreeNode("Id", "Title").Nodes);
        }

        #endregion
    }
}
