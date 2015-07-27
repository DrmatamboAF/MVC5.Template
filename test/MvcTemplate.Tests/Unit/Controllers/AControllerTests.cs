using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ProtectsFromOverposting(Controller controller, String postMethod, String value)
        {
            MethodInfo methodInfo = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == postMethod &&
                    method.IsDefined(typeof(HttpPostAttribute), false));

            CustomAttributeData actual = methodInfo
                .GetParameters()
                .First()
                .CustomAttributes
                .Single(attr => attr.AttributeType == typeof(BindAttribute));

            Assert.Equal(value, actual.NamedArguments.Single().TypedValue.Value);
            Assert.Equal("Exclude", actual.NamedArguments.Single().MemberName);
        }
    }
}
