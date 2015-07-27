using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        public Decimal Minimum { get; private set; }

        public GreaterThanAttribute(Int32 minimum)
            : this()
        {
            Minimum = minimum;
        }
        public GreaterThanAttribute(Double minimum)
            : this()
        {
            Minimum = Convert.ToDecimal(minimum);
        }
        private GreaterThanAttribute()
            : base(() => Validations.FieldMustBeGreaterThan)
        {
        }

        public override String FormatErrorMessage(String name)
        {
            return String.Format(ErrorMessageString, name, Minimum);
        }
        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return true;

            try
            {
                return Convert.ToDecimal(value) > Minimum;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
