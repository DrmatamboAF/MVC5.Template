using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MinValueAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "MinValue");
            MinValueAdapter adapter = new MinValueAdapter(metadata, new ControllerContext(), new MinValueAttribute(128));
            String errorMessage = new MinValueAttribute(128).FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("min", 128M);
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "range";

            Assert.Equal(expected.ValidationParameters["min"], actual.ValidationParameters["min"]);
            Assert.Equal(expected.ValidationParameters.Count, actual.ValidationParameters.Count);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
