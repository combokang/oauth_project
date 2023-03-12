using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Internal.SSO.Utilities.Extensions
{
    public static class IPrincipalExtensions
    {
        public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null) { return null; }

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);

            return claim == null ? null : claim.Value;
        }

        public static string GetUserId(this IPrincipal currentPrincipal)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == "UserId");

            return claim.Value;
        }
    }
}
