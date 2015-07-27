using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class MinValueAdapter : DataAnnotationsModelValidator<MinValueAttribute>
    {
        public MinValueAdapter(ModelMetadata metadata, ControllerContext context, MinValueAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ValidationParameters.Add("min", Attribute.Minimum);
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "range";

            return new[] { validationRule };
        }
    }
}
