using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Form;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class NumberValidatorTests
    {
        private NumberValidator validator;
        private ModelMetadata metadata;

        public NumberValidatorTests()
        {
            metadata = new DisplayNameMetadataProvider().GetMetadataForProperty(null, typeof(AccountView), "Username");
            validator = new NumberValidator(metadata, new ControllerContext());
        }

        #region Method: Validate(Object container)

        [Fact]
        public void Validate_DoesNotValidate()
        {
            Assert.Empty(validator.Validate(null));
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsDateValidationRule()
        {
            ModelClientValidationRule actual = validator.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule
            {
                ValidationType = "number",
                ErrorMessage = String.Format(Validations.FieldMustBeNumeric, metadata.GetDisplayName())
            };

            Assert.Equal(expected.ValidationParameters, actual.ValidationParameters);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
