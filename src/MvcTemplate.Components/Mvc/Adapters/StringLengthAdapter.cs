using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class StringLengthAdapter : StringLengthAttributeAdapter
    {
        public StringLengthAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            if (Attribute.MinimumLength == 0)
                Attribute.ErrorMessage = Validations.FieldMustNotExceedLength;
            else
                Attribute.ErrorMessage = Validations.FieldMustBeInRangeOfLength;
        }
    }
}