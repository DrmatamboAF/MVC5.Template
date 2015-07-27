using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class IntegerAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "Integer");
            IntegerAdapter adapter = new IntegerAdapter(metadata, new ControllerContext(), new IntegerAttribute());
            String errorMessage = new IntegerAttribute().FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "integer";

            Assert.Equal(expected.ValidationParameters.Count, actual.ValidationParameters.Count);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
