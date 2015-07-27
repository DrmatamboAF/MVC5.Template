using LightInject.Mvc;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Tests.Objects;
using MvcTemplate.Web;
using NSubstitute;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Web
{
    public class MvcApplicationTests : IDisposable
    {
        private MvcApplication application;

        public MvcApplicationTests()
        {
            application = Substitute.ForPartsOf<MvcApplication>();
            application.When(app => app.RegisterAreas()).DoNotCallBase();
            application.When(app => app.RegisterSiteMapProvider()).DoNotCallBase();
            application.When(app => app.RegisterGlobalizationProvider()).DoNotCallBase();

            DependencyResolver.SetResolver(Substitute.For<IDependencyResolver>());
            ModelValidatorProviders.Providers.Clear();
            RouteTable.Routes.LowercaseUrls = false;
            ModelMetadataProviders.Current = null;
            GlobalizationManager.Provider = null;
            Authorization.Provider = null;
            GlobalFilters.Filters.Clear();
            ModelBinders.Binders.Clear();
            ViewEngines.Engines.Clear();
            BundleTable.Bundles.Clear();
            MvcSiteMap.Provider = null;
            RouteTable.Routes.Clear();
        }
        public void Dispose()
        {
            application.Dispose();
        }

        #region Method: Application_Start()

        [Fact]
        public void Application_Start_RegistersCurrentDependencyResolver()
        {
            application.Application_Start();

            application.Received().RegisterCurrentDependencyResolver();
        }

        [Fact]
        public void Application_Start_RegistersGlobalizationProvider()
        {
            application.Application_Start();

            application.Received().RegisterGlobalizationProvider();
        }

        [Fact]
        public void Application_Start_RegistersModelMetadataProvider()
        {
            application.Application_Start();

            application.Received().RegisterModelMetadataProvider();
        }

        [Fact]
        public void Application_Start_RegistersDataTypeValidator()
        {
            application.Application_Start();

            application.Received().RegisterDataTypeValidator();
        }

        [Fact]
        public void Application_Start_RegistersSiteMapProvider()
        {
            application.Application_Start();

            application.Received().RegisterSiteMapProvider();
        }

        [Fact]
        public void Application_Start_RegistersAuthorization()
        {
            application.Application_Start();

            application.Received().RegisterAuthorization();
        }

        [Fact]
        public void Application_Start_RegistersModelBinders()
        {
            application.Application_Start();

            application.Received().RegisterModelBinders();
        }

        [Fact]
        public void Application_Start_RegistersViewEngine()
        {
            application.Application_Start();

            application.Received().RegisterViewEngine();
        }

        [Fact]
        public void Application_Start_RegistersAdapters()
        {
            application.Application_Start();

            application.Received().RegisterAdapters();
        }

        [Fact]
        public void Application_Start_RegistersFilters()
        {
            application.Application_Start();

            application.Received().RegisterFilters();
        }

        [Fact]
        public void Application_Start_RegistersBundles()
        {
            application.Application_Start();

            application.Received().RegisterBundles();
        }

        [Fact]
        public void Application_Start_RegistersAreas()
        {
            application.Application_Start();

            application.Received().RegisterAreas();
        }

        [Fact]
        public void Application_Start_RegistersRoute()
        {
            application.Application_Start();

            application.Received().RegisterRoute();
        }

        #endregion

        #region Method: Application_Error()

        [Fact(Skip = "Web configuration can not be edited.")]
        public void Application_Error()
        {
        }

        #endregion

        #region Method: RegisterCurrentDependencyResolver()

        [Fact]
        public void RegisterCurrentDependencyResolver_RegistersCurrentDependencyResolver()
        {
            Assert.IsNotType<LightInjectMvcDependencyResolver>(DependencyResolver.Current);

            application.RegisterCurrentDependencyResolver();

            Assert.IsType<LightInjectMvcDependencyResolver>(DependencyResolver.Current);
        }

        #endregion

        #region Method: RegisterGlobalizationProvider()

        [Fact]
        public void RegisterGlobalizationProvider_RegistersGlobalizationProvider()
        {
            IGlobalizationProvider globalization = Substitute.For<IGlobalizationProvider>();
            DependencyResolver.Current.GetService<IGlobalizationProvider>().Returns(globalization);

            application = Substitute.ForPartsOf<MvcApplication>();
            application.RegisterGlobalizationProvider();

            IGlobalizationProvider actual = GlobalizationManager.Provider;
            IGlobalizationProvider expected = globalization;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RegisterModelMetadataProvider()

        [Fact]
        public void RegisterModelMetadataProvider_RegistersModelMetadataProvider()
        {
            application.RegisterModelMetadataProvider();

            Assert.IsType<DisplayNameMetadataProvider>(ModelMetadataProviders.Current);
        }

        #endregion

        #region Method: RegisterDataTypeValidator()

        [Fact]
        public void RegisterDataTypeValidator_RemovesClientDataTypeModelValidatorProvider()
        {
            ClientDataTypeModelValidatorProvider provider = new ClientDataTypeModelValidatorProvider();
            ModelValidatorProviders.Providers.Add(provider);

            application.RegisterDataTypeValidator();

            Assert.False(ModelValidatorProviders.Providers.Contains(provider));
        }

        [Fact]
        public void RegisterDataTypeValidator_RegistersDataTypeValidatorProvider()
        {
            application.RegisterDataTypeValidator();

            ModelValidatorProviderCollection providers = ModelValidatorProviders.Providers;
            Type type = typeof(DataTypeValidatorProvider);

            Assert.Single(providers.Select(provider => provider.GetType()), type);
        }

        #endregion

        #region Method: RegisterSiteMapProvider()

        [Fact]
        public void RegisterSiteMapProvider_RegistersSiteMapProvider()
        {
            IMvcSiteMapProvider siteMap = Substitute.For<IMvcSiteMapProvider>();
            DependencyResolver.Current.GetService<IMvcSiteMapProvider>().Returns(siteMap);

            application = Substitute.ForPartsOf<MvcApplication>();
            application.RegisterSiteMapProvider();

            IMvcSiteMapProvider actual = MvcSiteMap.Provider;
            IMvcSiteMapProvider expected = siteMap;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RegisterAuthorization()

        [Fact]
        public void RegisterAuthorization_RegistersAuthorization()
        {
            IAuthorizationProvider provider = Substitute.For<IAuthorizationProvider>();
            DependencyResolver.Current.GetService<IAuthorizationProvider>().Returns(provider);

            application.RegisterAuthorization();

            IAuthorizationProvider actual = Authorization.Provider;
            IAuthorizationProvider expected = provider;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RegisterAuthorization_RefreshesAuthorizationProvider()
        {
            IAuthorizationProvider provider = Substitute.For<IAuthorizationProvider>();
            DependencyResolver.Current.GetService<IAuthorizationProvider>().Returns(provider);

            application.RegisterAuthorization();

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: RegisterModelBinders()

        [Fact]
        public void RegisterModelBinders_RegistersModelBinders()
        {
            Assert.Null(ModelBinders.Binders[typeof(String)]);

            application.RegisterModelBinders();

            IModelBinder actual = ModelBinders.Binders[typeof(String)];

            Assert.IsType<TrimmingModelBinder>(actual);
        }

        #endregion

        #region Method: RegisterViewEngine()

        [Fact]
        public void RegisterViewEngine_RemovesUnnecessaryViewEngines()
        {
            IViewEngine engine = Substitute.For<IViewEngine>();
            ViewEngines.Engines.Add(engine);

            application.RegisterViewEngine();

            Assert.False(ViewEngines.Engines.Contains(engine));
        }

        [Fact]
        public void RegisterViewEngine_RegistersViewEngine()
        {
            application.RegisterViewEngine();

            IViewEngine actual = ViewEngines.Engines.Single();

            Assert.IsType<ViewEngine>(actual);
        }

        #endregion

        #region Method: RegisterAdapters()

        [Theory]
        [InlineData("Range", typeof(RangeAdapter))]
        [InlineData("Digits", typeof(DigitsAdapter))]
        [InlineData("EqualTo", typeof(EqualToAdapter))]
        [InlineData("Integer", typeof(IntegerAdapter))]
        [InlineData("Required", typeof(RequiredAdapter))]
        [InlineData("MinValue", typeof(MinValueAdapter))]
        [InlineData("MaxValue", typeof(MaxValueAdapter))]
        [InlineData("MinLength", typeof(MinLengthAdapter))]
        [InlineData("GreaterThan", typeof(GreaterThanAdapter))]
        [InlineData("EmailAddress", typeof(EmailAddressAdapter))]
        [InlineData("StringLength", typeof(StringLengthAdapter))]
        public void RegisterAdapters_RegistersAdapter(String property, Type adapterType)
        {
            DataAnnotationsModelValidatorProvider provider = new DataAnnotationsModelValidatorProvider();
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), property);

            application.RegisterAdapters();

            ModelValidator adapter = provider
                .GetValidators(metadata, new ControllerContext())
                .SingleOrDefault(validator => validator.GetType() == adapterType);

            Assert.NotNull(adapter);
        }

        #endregion

        #region Method: RegisterFilters()

        [Fact]
        public void RegisterFilters_RegistersExceptionFilter()
        {
            IExceptionFilter filter = Substitute.For<IExceptionFilter>();
            DependencyResolver.Current.GetService<IExceptionFilter>().Returns(filter);

            application.RegisterFilters();

            Object actual = GlobalFilters.Filters.Single().Instance;
            Object expected = filter;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RegisterBundles()

        [Fact]
        public void RegisterBundles_RegistersBundles()
        {
            IBundleConfig bundleConfig = Substitute.For<IBundleConfig>();
            DependencyResolver.Current.GetService<IBundleConfig>().Returns(bundleConfig);

            application.RegisterBundles();

            bundleConfig.Received().RegisterBundles(BundleTable.Bundles);
        }

        #endregion

        #region Method: RegisterAreas()

        [Fact(Skip = "Cannot be called during the application's pre-start initialization stage.")]
        public void RegisterAreas_RegistersAreas()
        {
        }

        #endregion

        #region Method: RegisterRoute()

        [Fact]
        public void RegisterRoute_RegistersLowercaseUrls()
        {
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            DependencyResolver.Current.GetService<IRouteConfig>().Returns(routeConfig);

            application.RegisterRoute();

            Assert.True(RouteTable.Routes.LowercaseUrls);
        }

        [Fact]
        public void RegisterRoute_RegistersRoute()
        {
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            DependencyResolver.Current.GetService<IRouteConfig>().Returns(routeConfig);

            application.RegisterRoute();

            routeConfig.Received().RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}
