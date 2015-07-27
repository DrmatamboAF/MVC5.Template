using MvcTemplate.Components.Security;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class AuthorizeAsAttributeTests
    {
        #region Constructor: AuthorizeAsAttribute(String action)

        [Fact]
        public void AuthorizeAsAttribute_SetsAction()
        {
            String actual = new AuthorizeAsAttribute("Action").Action;
            String expected = "Action";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
