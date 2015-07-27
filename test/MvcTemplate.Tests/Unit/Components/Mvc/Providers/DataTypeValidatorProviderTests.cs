using MvcTemplate.Components.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DataTypeValidatorProviderTests
    {
        private DataTypeValidatorProvider provider;

        public DataTypeValidatorProviderTests()
        {
            provider = new DataTypeValidatorProvider();
        }

        #region Method: GetValidators(ModelMetadata metadata, ControllerContext context)

        [Fact]
        public void GetValidators_GetsNoValidators()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Id");

            Assert.Empty(provider.GetValidators(metadata, new ControllerContext()));
        }

        [Fact]
        public void GetValidators_GetsDateValidator()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Date");

            IEnumerable<Type> actual = provider.GetValidators(metadata, new ControllerContext()).Select(validator => validator.GetType());
            IEnumerable<Type> expected = new[] { typeof(DateValidator) };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValidators_GetsNumericValidator()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Numeric");

            IEnumerable<Type> actual = provider.GetValidators(metadata, new ControllerContext()).Select(validator => validator.GetType());
            IEnumerable<Type> expected = new[] { typeof(NumberValidator) };

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
