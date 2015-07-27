using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MaxValueAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsMaxRangeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "MaxValue");
            MaxValueAdapter adapter = new MaxValueAdapter(metadata, new ControllerContext(), new MaxValueAttribute(128));
            String errorMessage = new MaxValueAttribute(128).FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("max", 128M);
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "range";

            Assert.Equal(expected.ValidationParameters["max"], actual.ValidationParameters["max"]);
            Assert.Equal(expected.ValidationParameters.Count, actual.ValidationParameters.Count);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
