using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using System;
using System.Web.Mvc;
using Xunit;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class RangeAdapterTests
    {
        #region Constructor: RangeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)

        [Fact]
        public void RangeAdapter_SetsRangeErrorMessage()
        {
            DataAnnotations.RangeAttribute attribute = new DataAnnotations.RangeAttribute(0, 128);
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "Range");
            new RangeAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldMustBeInRange;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
