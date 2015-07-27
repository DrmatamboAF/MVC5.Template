using MvcTemplate.Resources;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class EqualToAdapter : DataAnnotationsModelValidator<EqualToAttribute>
    {
        public EqualToAdapter(ModelMetadata metadata, ControllerContext context, EqualToAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            Attribute.OtherPropertyDisplayName = ResourceProvider.GetPropertyTitle(Metadata.ContainerType, Attribute.OtherPropertyName);

            ModelClientValidationRule validationRule = new ModelClientValidationRule();
            validationRule.ValidationParameters.Add("other", "*." + Attribute.OtherPropertyName);
            validationRule.ErrorMessage = ErrorMessage;
            validationRule.ValidationType = "equalto";

            return new[] { validationRule };
        }
    }
}
