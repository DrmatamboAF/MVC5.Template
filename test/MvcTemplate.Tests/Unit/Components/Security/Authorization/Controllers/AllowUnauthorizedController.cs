using MvcTemplate.Components.Security;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowUnauthorized]
    public abstract class AllowUnauthorizedController : AuthorizedController
    {
    }
}
