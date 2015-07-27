using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class GreaterThanAdapter : DataAnnotationsModelValidator<GreaterThanAttribute>
    {
        public GreaterThanAdapter(ModelMetadata metadata, ControllerContext context, GreaterThanAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ValidationParameters.Add("min", Attribute.Minimum);
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "greater";

            return new[] { validationRule };
        }
    }
}
