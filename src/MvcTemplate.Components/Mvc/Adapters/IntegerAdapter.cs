using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class IntegerAdapter : DataAnnotationsModelValidator<IntegerAttribute>
    {
        public IntegerAdapter(ModelMetadata metadata, ControllerContext context, IntegerAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "integer";

            return new[] { validationRule };
        }
    }
}
