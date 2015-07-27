using LightInject;
using LightInject.Mvc;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Web.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcTemplate.Web
{
    public class MvcApplication : HttpApplication
    {
        public void Application_Start()
        {
            RegisterCurrentDependencyResolver();
            RegisterGlobalizationProvider();
            RegisterModelMetadataProvider();
            RegisterDataTypeValidator();
            RegisterSiteMapProvider();
            RegisterAuthorization();
            RegisterModelBinders();
            RegisterViewEngine();
            RegisterAdapters();
            RegisterFilters();
            RegisterBundles();
            RegisterAreas();
            RegisterRoute();
        }
        public void Application_Error()
        {
            if (!ErrorHandlingEnabled()) return;

            RouteValueDictionary routeValues = new RouteValueDictionary(Request.RequestContext.RouteData.Values);
            HttpException httpException = Server.GetLastError() as HttpException;
            UrlHelper urlHelper = new UrlHelper(Request.RequestContext);

            routeValues["controller"] = "Home";
            routeValues["action"] = "Error";
            routeValues["area"] = "";
            Server.ClearError();

            if (httpException != null && httpException.GetHttpCode() == 404)
                routeValues["action"] = "NotFound";

            Response.TrySkipIisCustomErrors = true;
            Response.Redirect(urlHelper.RouteUrl(routeValues));
        }

        public virtual void RegisterCurrentDependencyResolver()
        {
            MainContainer container = new MainContainer();
            container.RegisterControllers(typeof(BaseController).Assembly);
            container.RegisterServices();
            container.EnableMvc();

            DependencyResolver.SetResolver(new LightInjectMvcDependencyResolver(container));
        }
        public virtual void RegisterGlobalizationProvider()
        {
            GlobalizationManager.Provider = DependencyResolver.Current.GetService<IGlobalizationProvider>();
        }
        public virtual void RegisterModelMetadataProvider()
        {
            ModelMetadataProviders.Current = new DisplayNameMetadataProvider();
        }
        public virtual void RegisterDataTypeValidator()
        {
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.SingleOrDefault(x => x is ClientDataTypeModelValidatorProvider));
            ModelValidatorProviders.Providers.Add(new DataTypeValidatorProvider());
        }
        public virtual void RegisterSiteMapProvider()
        {
            MvcSiteMap.Provider = DependencyResolver.Current.GetService<IMvcSiteMapProvider>();
        }
        public virtual void RegisterAuthorization()
        {
            Authorization.Provider = DependencyResolver.Current.GetService<IAuthorizationProvider>();
            Authorization.Provider.Refresh();
        }
        public virtual void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(String), new TrimmingModelBinder());
        }
        public virtual void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ViewEngine());
        }
        public virtual void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeAttribute), typeof(RangeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DigitsAttribute), typeof(DigitsAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EqualToAttribute), typeof(EqualToAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(IntegerAttribute), typeof(IntegerAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinValueAttribute), typeof(MinValueAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxValueAttribute), typeof(MaxValueAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinLengthAttribute), typeof(MinLengthAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(GreaterThanAttribute), typeof(GreaterThanAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(EmailAddressAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(StringLengthAdapter));
        }
        public virtual void RegisterFilters()
        {
            GlobalFilters.Filters.Add(DependencyResolver.Current.GetService<IExceptionFilter>());
        }
        public virtual void RegisterBundles()
        {
            DependencyResolver.Current.GetService<IBundleConfig>().RegisterBundles(BundleTable.Bundles);
        }
        public virtual void RegisterAreas()
        {
            AreaRegistration.RegisterAllAreas();
        }
        public virtual void RegisterRoute()
        {
            RouteTable.Routes.LowercaseUrls = true;
            DependencyResolver.Current.GetService<IRouteConfig>().RegisterRoutes(RouteTable.Routes);
        }

        private Boolean ErrorHandlingEnabled()
        {
            CustomErrorsSection customErrors = (CustomErrorsSection)WebConfigurationManager.GetSection("system.web/customErrors");
            if (customErrors == null) return false;

            if (customErrors.Mode == CustomErrorsMode.RemoteOnly && !Request.IsLocal) return true;
            if (customErrors.Mode == CustomErrorsMode.On) return true;

            return false;
        }
    }
}
