using MvcTemplate.Components.Extensions.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Mvc
{
    public class ModelStateDictionaryExtensionsTests
    {
        private ModelStateDictionary modelState;

        public ModelStateDictionaryExtensionsTests()
        {
            modelState = new ModelStateDictionary();
        }

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, Exception exception)

        [Fact]
        public void AddModelError_AddsModelExceptionKey()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.StringField, new Exception());

            String actual = modelState.Single().Key;
            String expected = "Child.StringField";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_AddsModelException()
        {
            Exception exception = new Exception();
            modelState.AddModelError<AllTypesView>(model => model.Child.StringField, exception);

            Exception actual = modelState.Single().Value.Errors.Single().Exception;
            Exception expected = exception;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String errorMessage)

        [Fact]
        public void AddModelError_AddsModelErrorKey()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.NullableByteField, "Test error");

            String expected = "Child.NullableByteField";
            String actual = modelState.Single().Key;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_AddsModelErrorMessage()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.NullableByteField, "Test error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String format, Object[] args)

        [Fact]
        public void AddModelError_Format_AddsModelErrorKey()
        {
            modelState.AddModelError<AllTypesView>(model => model.Int32Field, "Test {0}", "error");

            String actual = modelState.Single().Key;
            String expected = "Int32Field";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_Format_AddsFormattedModelErrorMessage()
        {
            modelState.AddModelError<AllTypesView>(model => model.Int32Field, "Test {0}", "error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
