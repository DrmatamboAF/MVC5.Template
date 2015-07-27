using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MinValueAttributeTests
    {
        private MinValueAttribute attribute;

        public MinValueAttributeTests()
        {
            attribute = new MinValueAttribute(12.56);
        }

        #region Constructor: MinValueAttribute(Int32 minimum)

        [Fact]
        public void MinValueAttribute_SetsMinimumFromInteger()
        {
            Decimal actual = new MinValueAttribute(10).Minimum;
            Decimal expected = 10M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: MinValueAttribute(Double minimum)

        [Fact]
        public void MinValueAttribute_SetsMinimumFromDouble()
        {
            Decimal actual = new MinValueAttribute(12.56).Minimum;
            Decimal expected = 12.56M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            attribute = new MinValueAttribute(10);

            String expected = String.Format(Validations.FieldMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            attribute = new MinValueAttribute(12.56);

            String expected = String.Format(Validations.FieldMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

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
        public void IsValid_StringValueIsValid()
        {
            Assert.True(attribute.IsValid("100"));
        }

        [Fact]
        public void IsValid_StringValueIsNotValid()
        {
            Assert.False(attribute.IsValid("1"));
        }

        [Fact]
        public void IsValid_LowerValueIsNotValid()
        {
            Assert.False(attribute.IsValid(12.559));
        }

        [Fact]
        public void IsValid_EqualValueIsValid()
        {
            Assert.True(attribute.IsValid(12.56));
        }

        [Fact]
        public void IsValid_GreaterValueIsValid()
        {
            Assert.True(attribute.IsValid(13));
        }

        [Fact]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.False(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
