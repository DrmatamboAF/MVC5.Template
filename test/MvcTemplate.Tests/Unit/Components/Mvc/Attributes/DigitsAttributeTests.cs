using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DigitsAttributeTests
    {
        private DigitsAttribute attribute;

        public DigitsAttributeTests()
        {
            attribute = new DigitsAttribute();
        }

        #region Constructor: DigitsAttribute()

        [Fact]
        public void DigitsAttribute_SetsErrorMessage()
        {
            attribute = new DigitsAttribute();

            String expected = String.Format(Validations.FieldMustBeOnlyInDigits, "Test");
            String actual = attribute.FormatErrorMessage("Test");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: IsValid(Object value)

        [Fact]
        public void IsValid_NullValueIsValid()
        {
            Assert.True(attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_RealValueIsNotValid()
        {
            Assert.False(attribute.IsValid(12.549));
        }

        [Fact]
        public void IsValid_StringValueIsNotValid()
        {
            Assert.False(attribute.IsValid("+1402"));
        }

        [Fact]
        public void IsValid_NegativeValueIsNotValid()
        {
            Assert.False(attribute.IsValid(-2546798));
        }

        [Fact]
        public void IsValid_DigitsValue()
        {
            Assert.True(attribute.IsValid("92233720368547758074878484887777"));
        }

        #endregion
    }
}
