using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class DigitsAttribute : ValidationAttribute
    {
        public DigitsAttribute() : base(() => Validations.FieldMustBeOnlyInDigits)
        {
        }

        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return true;

            return Regex.IsMatch(value.ToString(), "^[0-9]+$");
        }
    }
}
