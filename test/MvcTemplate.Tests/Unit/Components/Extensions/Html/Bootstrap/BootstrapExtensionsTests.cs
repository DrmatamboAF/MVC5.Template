using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapExtensionsTests : IDisposable
    {
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel model;

        public BootstrapExtensionsTests()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            html = HtmlHelperFactory.CreateHtmlHelper(new BootstrapModel());
            model = html.ViewData.Model;
        }
        public void Dispose()
        {
            GlobalizationManager.Provider = null;
        }

        #region Extension method: FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormLabelFor_OnNotMemberExpressionThrows()
        {
            Exception exception = Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.GetType()));

            String expected = "Expression must be a member expression.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.Required).ToString();
            String expected =
                "<label for=\"Relation_Required\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabelOnValueTypes()
        {
            String actual = html.FormLabelFor(x => x.Relation.RequiredValue).ToString();
            String expected =
                "<label for=\"Relation_RequiredValue\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequired).ToString();
            String expected =
                "<label for=\"Relation_NotRequired\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabelOnNullableValueTypes()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequiredNullableValue).ToString();
            String expected =
                "<label for=\"Relation_NotRequiredNullableValue\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextBoxFor_FormsNotAutocompletableTextBox()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.NotEditable).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"NotEditable\" name=\"NotEditable\" type=\"text\" value=\"{0}\" />",
                model.NotEditable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Fact]
        public void FormTextBoxFor_Format_FormsNotAutocompletableTextBox()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.NotEditable, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"NotEditable\" name=\"NotEditable\" type=\"text\" value=\"{0}\" />",
                model.NotEditable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_FormatsTextBoxValue()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Attributes_MergesClassAttributes()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_FormsNotAutocompletableTextBox()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.NotEditable, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"NotEditable\" name=\"NotEditable\" type=\"text\" value=\"{0}\" />",
                model.NotEditable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Format_Attributes_MergesClassAttributes()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, null, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_FormsNotAutocompletableTextBox()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, null, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, null, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.NotEditable, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"NotEditable\" name=\"NotEditable\" type=\"text\" value=\"{0}\" />",
                model.NotEditable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_FormatsTextBoxValue()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}", null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Fact]
        public void FormPasswordFor_FormsNotAutocompletablePasswordInput()
        {
            String expected = "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Required\" name=\"Relation.Required\" type=\"password\" />";
            String actual = html.FormPasswordFor(x => x.Relation.Required).ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextAreaFor_FormsNotAutocompletableTextArea()
        {
            String actual = html.FormTextAreaFor(x => x.Relation.NotRequired).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextAreaFor(x => x.NotEditable).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"NotEditable\" name=\"NotEditable\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.NotEditable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableTrue\" name=\"EditableTrue\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDatePicker()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDatePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy.MM.dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDatePickerFor_FormsDatePickerWtihAttributes()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDatePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy.MM.dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDateTimePicker()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy.MM.dd HH:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDatePickerFor_FormsDateTimePickerWithAttributes()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy.MM.dd HH:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
