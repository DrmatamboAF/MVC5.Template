using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MaxValueAttribute : ValidationAttribute
    {
        public Decimal Maximum { get; private set; }

        public MaxValueAttribute(Int32 maximum) : this()
        {
            Maximum = maximum;
        }
        public MaxValueAttribute(Double maximum) : this()
        {
            Maximum = Convert.ToDecimal(maximum);
        }
        private MaxValueAttribute() : base(() => Validations.FieldMustBeLessOrEqualTo)
        {
        }

        public override String FormatErrorMessage(String name)
        {
            return String.Format(ErrorMessageString, name, Maximum);
        }
        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return true;

            try
            {
                return Convert.ToDecimal(value) <= Maximum;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
