using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MaxValueAttributeTests
    {
        private MaxValueAttribute attribute;

        public MaxValueAttributeTests()
        {
            attribute = new MaxValueAttribute(12.56);
        }

        #region Constructor: MaxValueAttribute(Int32 maximum)

        [Fact]
        public void MaxValueAttribute_SetsMaximumFromInteger()
        {
            Decimal actual = new MaxValueAttribute(10).Maximum;
            Decimal expected = 10M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: MaxValueAttribute(Double maximum)

        [Fact]
        public void MaxValueAttribute_SetsMaximumFromDouble()
        {
            Decimal actual = new MaxValueAttribute(12.56).Maximum;
            Decimal expected = 12.56M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            attribute = new MaxValueAttribute(10);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            attribute = new MaxValueAttribute(13.44);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
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
            Assert.True(attribute.IsValid("5"));
        }

        [Fact]
        public void IsValid_StringValueIsNotValid()
        {
            Assert.False(attribute.IsValid("100"));
        }

        [Fact]
        public void IsValid_GreaterValueIsNotValid()
        {
            Assert.False(attribute.IsValid(13));
        }

        [Fact]
        public void IsValid_EqualValueIsValid()
        {
            Assert.True(attribute.IsValid(12.56));
        }

        [Fact]
        public void IsValid_LowerValueIsValid()
        {
            Assert.True(attribute.IsValid(12.559));
        }

        [Fact]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.False(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
