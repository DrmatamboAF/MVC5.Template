using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class EqualToAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_SetsOtherPropertyDisplayName()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAttribute attribute = new EqualToAttribute("StringLength");
            attribute.OtherPropertyDisplayName = null;

            new EqualToAdapter(metadata, new ControllerContext(), attribute).GetClientValidationRules();

            String expected = ResourceProvider.GetPropertyTitle(typeof(AdaptersModel), "EqualTo");
            String actual = attribute.OtherPropertyDisplayName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetClientValidationRules_ReturnsEqualToValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAdapter adapter = new EqualToAdapter(metadata, new ControllerContext(), new EqualToAttribute("StringLength"));
            String errorMessage = new EqualToAttribute("StringLength").FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("other", "*.StringLength");
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "equalto";

            Assert.Equal(expected.ValidationParameters["other"], actual.ValidationParameters["other"]);
            Assert.Equal(expected.ValidationParameters.Count, actual.ValidationParameters.Count);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
