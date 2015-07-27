using MvcTemplate.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class BootstrapExtensions
    {
        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder requiredSpan = new TagBuilder("span");
            TagBuilder label = new TagBuilder("label");
            requiredSpan.AddCssClass("require");

            if (expression.IsRequired())
                requiredSpan.InnerHtml = "*";

            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression) + requiredSpan;

            return new MvcHtmlString(label.ToString());
        }

        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormTextBoxFor(expression, null, null);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)
        {
            return html.FormTextBoxFor(expression, format, null);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return html.FormTextBoxFor(expression, null, htmlAttributes);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)
        {
            RouteValueDictionary attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control");

            return html.TextBoxFor(expression, format, attributes);
        }

        public static MvcHtmlString FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return html.PasswordFor(expression, new { @class = "form-control", autocomplete = "off" });
        }

        public static MvcHtmlString FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            RouteValueDictionary attributes = FormHtmlAttributes(expression, new { rows = 6 }, "form-control");

            return html.TextAreaFor(expression, attributes);
        }

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormDatePickerFor(expression, null);
        }
        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            RouteValueDictionary attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control datepicker");

            return html.TextBoxFor(expression, "{0:d}", attributes);
        }

        public static MvcHtmlString FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormDateTimePickerFor(expression, null);
        }
        public static MvcHtmlString FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            RouteValueDictionary attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control datetimepicker");

            return html.TextBoxFor(expression, "{0:g}", attributes);
        }

        private static RouteValueDictionary FormHtmlAttributes(LambdaExpression expression, Object attributes, String cssClass)
        {
            RouteValueDictionary htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            if (!htmlAttributes.ContainsKey("autocomplete")) htmlAttributes.Add("autocomplete", "off");
            htmlAttributes["class"] = (cssClass + " " + htmlAttributes["class"]).Trim();
            if (htmlAttributes.ContainsKey("readonly")) return htmlAttributes;

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null && memberExpression.Member.IsDefined(typeof(EditableAttribute), false))
            {
                EditableAttribute editable = memberExpression.Member.GetCustomAttribute<EditableAttribute>(false);
                if (!editable.AllowEdit) htmlAttributes.Add("readonly", "readonly");
            }

            return htmlAttributes;
        }
        private static Boolean IsRequired(this LambdaExpression expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression.");

            if (!AllowsNullValues(expression.ReturnType))
                return true;

            return memberExpression.Member.IsDefined(typeof(RequiredAttribute), false);
        }
        private static Boolean AllowsNullValues(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}
