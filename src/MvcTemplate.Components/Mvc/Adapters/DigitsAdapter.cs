using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DigitsAdapter : DataAnnotationsModelValidator<DigitsAttribute>
    {
        public DigitsAdapter(ModelMetadata metadata, ControllerContext context, DigitsAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "digits";

            return new[] { validationRule };
        }
    }
}
