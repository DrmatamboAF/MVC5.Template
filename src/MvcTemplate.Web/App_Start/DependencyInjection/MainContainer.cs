using LightInject;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System.Data.Entity;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcTemplate.Web.DependencyInjection
{
    public class MainContainer : ServiceContainer
    {
        public virtual void RegisterServices()
        {
            Register<DbContext, Context>();
            Register<IUnitOfWork, UnitOfWork>();

            Register<ILogger, Logger>();
            Register<IAuditLogger, AuditLogger>();

            Register<IHasher, BCrypter>();
            Register<IMailClient, SmtpMailClient>();

            Register<IRouteConfig, RouteConfig>();
            Register<IBundleConfig, BundleConfig>();
            Register<IExceptionFilter, ExceptionFilter>();

            Register<IMvcSiteMapParser, MvcSiteMapParser>();
            Register<IMvcSiteMapProvider>(factory => new MvcSiteMapProvider(
                 HostingEnvironment.MapPath("~/Mvc.sitemap"),
                 factory.GetInstance<IMvcSiteMapParser>()));

            Register<IGlobalizationProvider>(factory =>
                new GlobalizationProvider(HostingEnvironment.MapPath("~/Globalization.xml")));
            RegisterInstance<IAuthorizationProvider>(new AuthorizationProvider(typeof(BaseController).Assembly));

            Register<IRoleService, RoleService>();
            Register<IAccountService, AccountService>();

            Register<IRoleValidator, RoleValidator>();
            Register<IAccountValidator, AccountValidator>();
        }
    }
}
