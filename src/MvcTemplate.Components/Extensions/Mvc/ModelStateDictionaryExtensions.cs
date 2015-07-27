using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, Exception exception)
        {
            modelState.AddModelError(GetExpressionText(expression), exception);
        }

        public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String errorMessage)
        {
            modelState.AddModelError(GetExpressionText(expression), errorMessage);
        }
        public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String format, params Object[] args)
        {
            modelState.AddModelError(GetExpressionText(expression), String.Format(format, args));
        }

        private static String GetExpressionText(LambdaExpression expression)
        {
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null && unaryExpression.Operand is MemberExpression)
                return ExpressionHelper.GetExpressionText(Expression.Lambda(unaryExpression.Operand, expression.Parameters[0]));

            return ExpressionHelper.GetExpressionText(expression);
        }
    }
}
