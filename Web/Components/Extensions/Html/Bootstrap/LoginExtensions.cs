﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public static class LoginExtensions
    {
        public static MvcHtmlString LoginUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-user");
            TagBuilder addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon");
            addon.InnerHtml = icon.ToString();

            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes["placeholder"] = ResourceProvider.GetPropertyTitle(expression);
            attributes["class"] = "form-control";

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString LoginPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-lock");
            TagBuilder addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon lock-span");
            addon.InnerHtml = icon.ToString();

            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes["class"] = "form-control";
            attributes["placeholder"] = ResourceProvider.GetPropertyTitle(expression);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.PasswordFor(expression, attributes)));
        }
        public static MvcHtmlString LoginLanguageSelect<TModel>(this HtmlHelper<TModel> html)
        {
            TagBuilder addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon flag-span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-flag");
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("id", "TempLanguage");
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control");
            TagBuilder select = new TagBuilder("select");
            select.MergeAttribute("id", "Language");

            addon.InnerHtml = icon.ToString();
            Dictionary<String, String> languages = new Dictionary<String, String>()
            {
                { "en-GB", "English" },
                { "lt-LT", "Lietuvių" }
            };
            foreach (KeyValuePair<String, String> language in languages)
            {
                TagBuilder option = new TagBuilder("option");
                option.MergeAttribute("value", language.Key);
                option.InnerHtml = language.Value;
                select.InnerHtml += option.ToString();
            }

            return new MvcHtmlString(String.Format("{0}{1}{2}", addon, input, select));
        }

        public static MvcHtmlString LoginSubmit<TModel>(this HtmlHelper<TModel> html)
        {
            TagBuilder formActions = new TagBuilder("div");
            formActions.AddCssClass("login-form-actions");
            TagBuilder submit = new TagBuilder("input");
            submit.AddCssClass("btn btn-block btn-primary btn-default");
            submit.MergeAttribute("value", Resources.Shared.Resources.Login);
            submit.MergeAttribute("type", "submit");
            formActions.InnerHtml = submit.ToString();

            return new MvcHtmlString(formActions.ToString());
        }
    }
}
