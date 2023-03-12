using MiddleOffice.Utilities.Extensions;
using MiddleOffice.Web.HttpClients.Sso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MiddleOffice.Web.Filters
{
    public class EdenredSsoAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string _permission { get; private set; }
        public EdenredSsoAuthorizeAttribute(string permissionText)
        {
            _permission = permissionText;
        }

        public EdenredSsoAuthorizeAttribute()
        {
            _permission = string.Empty;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Auth", action = "Logout" }));
                return;
            }

            if (string.IsNullOrEmpty(_permission)) { return; }

            var userService = context.HttpContext.RequestServices.GetService<UserService>();
            var isAjaxCall = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            try
            {
                var status = await userService.CheckPermissionAsync(_permission);

                if (status == HttpStatusCode.OK) { return; }

                if (status == HttpStatusCode.Unauthorized)
                {
                    if (isAjaxCall)
                    {
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Auth", action = "Logout" }));
                    }

                    return;
                }

                if (status == HttpStatusCode.Forbidden)
                {
                    if (isAjaxCall)
                    {
                        context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "ForbiddenPage" }));
                    }

                    return;
                }

                throw new Exception("No match result");
            }
            catch (Exception)
            {
                if (isAjaxCall)
                {
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.InternalServerError);
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "InternalServerErrorPage" }));
                }
            }
        }
    }
}
