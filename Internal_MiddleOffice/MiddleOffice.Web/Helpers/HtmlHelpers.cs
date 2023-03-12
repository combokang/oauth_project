using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions = "", string cssClass = "active")
        {
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return (actions == string.Empty || acceptedActions.Contains(currentAction)) && acceptedControllers.Contains(currentController) ?
                cssClass :
                string.Empty;
        }

        public static string GetDisplayName<TSource, TProperty>(this IHtmlHelper htmlHelper, Expression<Func<TSource, TProperty>> expression)
        {
            var type = typeof(TSource);
            var memberExpression = (MemberExpression)expression.Body;
            var propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);
            var attribute = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            return attribute == null ? propertyName : attribute.Name;
        }
    }
}
