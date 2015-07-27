using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class TrimmingModelBinderTests
    {
        private NameValueCollection nameValueCollection;
        private ControllerContext controllerContext;
        private ModelBindingContext bindingContext;
        private TrimmingModelBinder binder;

        public TrimmingModelBinderTests()
        {
            nameValueCollection = new NameValueCollection();
            controllerContext = new ControllerContext();
            bindingContext = new ModelBindingContext();
            bindingContext.ModelName = "Trimmed";
            binder = new TrimmingModelBinder();

            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(BindersModel), bindingContext.ModelName);
        }

        #region Method: BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)

        [Fact]
        public void BindModel_ReturnNullThenThereIsNoValueProvider()
        {
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.Null(binder.BindModel(controllerContext, bindingContext));
        }

        [Fact]
        public void BindModel_OnNullValueReturnsNull()
        {
            nameValueCollection.Add(bindingContext.ModelName, null);
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.Null(binder.BindModel(controllerContext, bindingContext));
        }

        [Fact]
        public void BindModel_DoesNotTrimPropertyWithNotTrimmedAttribute()
        {
            bindingContext.ModelName = "NotTrimmed";
            nameValueCollection.Add(bindingContext.ModelName, "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);
            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(BindersModel), "NotTrimmed");

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = "  Trimmed text  ";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModel_TrimsBindedModelsProperty()
        {
            nameValueCollection.Add(bindingContext.ModelName, "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = "Trimmed text";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
