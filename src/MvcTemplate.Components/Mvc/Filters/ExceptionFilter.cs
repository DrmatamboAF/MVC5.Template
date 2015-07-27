using MvcTemplate.Components.Logging;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger Logger { get; set; }

        public ExceptionFilter(ILogger logger)
        {
            Logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            while (exception.InnerException != null)
                exception = exception.InnerException;

            String message = String.Format("{0}: {1}{2}{3}",
                    exception.GetType(),
                    exception.Message,
                    Environment.NewLine,
                    exception.StackTrace);

            Logger.Log(filterContext.HttpContext.User.Identity.Name, message);
        }
    }
}
