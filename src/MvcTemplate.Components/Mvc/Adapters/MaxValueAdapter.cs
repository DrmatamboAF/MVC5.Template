using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class MaxValueAdapter : DataAnnotationsModelValidator<MaxValueAttribute>
    {
        public MaxValueAdapter(ModelMetadata metadata, ControllerContext context, MaxValueAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ValidationParameters.Add("max", Attribute.Maximum);
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "range";

            return new[] { validationRule };
        }
    }
}
