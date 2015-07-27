using MvcTemplate.Objects;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class RoleViewTests
    {
        #region Constructor: RoleView()

        [Fact]
        public void RoleView_PrivilegesTreeIsNotNull()
        {
            Assert.NotNull(new RoleView().PrivilegesTree);
        }

        #endregion
    }
}
