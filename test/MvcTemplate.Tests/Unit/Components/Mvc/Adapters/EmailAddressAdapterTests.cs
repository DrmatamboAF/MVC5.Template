using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class EmailAddressAdapterTests
    {
        private EmailAddressAttribute attribute;
        private EmailAddressAdapter adapter;
        private ModelMetadata metadata;

        public EmailAddressAdapterTests()
        {
            attribute = new EmailAddressAttribute();
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "EmailAddress");
            adapter = new EmailAddressAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: EmailAddressAdapter(ModelMetadata metadata, ControllerContext context, EmailAddressAttribute attribute)

        [Fact]
        public void EmailAddressAdapter_SetsErrorMessage()
        {
            String expected = Validations.FieldIsNotValidEmail;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsEmailValidationRule()
        {
            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule
            {
                ErrorMessage = String.Format(Validations.FieldIsNotValidEmail, metadata.GetDisplayName()),
                ValidationType = "email"
            };

            Assert.Equal(expected.ValidationParameters, actual.ValidationParameters);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
