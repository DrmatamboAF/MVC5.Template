using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ExceptionFilterTests
    {
        private ExceptionFilter filter;
        private Exception exception;
        private ILogger logger;

        public ExceptionFilterTests()
        {
            exception = GenerateException();
            logger = Substitute.For<ILogger>();
            filter = new ExceptionFilter(logger);
        }

        #region Method: OnException(ExceptionContext filterContext)

        [Fact]
        public void OnException_LogsFormattedException()
        {
            ExceptionContext context = new ExceptionContext
            {
                HttpContext = HttpContextFactory.CreateHttpContextBase(),
                Exception = exception
            };

            filter.OnException(context);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                exception.GetType(),
                exception.Message,
                Environment.NewLine,
                exception.StackTrace);

            logger.Received().Log(context.HttpContext.User.Identity.Name, expectedMessage);
        }

        [Fact]
        public void OnException_LogsOnlyInnerMostException()
        {
            ExceptionContext context = new ExceptionContext
            {
                HttpContext = HttpContextFactory.CreateHttpContextBase(),
                Exception = new Exception("O", exception)
            };

            filter.OnException(context);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                context.Exception.InnerException.GetType(),
                context.Exception.InnerException.Message,
                Environment.NewLine,
                context.Exception.InnerException.StackTrace);

            logger.Received().Log(context.HttpContext.User.Identity.Name, expectedMessage);
        }

        #endregion

        #region Test helpers

        private Exception GenerateException()
        {
            try
            {
                return new Exception(((Object)null).ToString());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        #endregion
    }
}
