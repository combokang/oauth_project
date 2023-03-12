using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MiddleOffice.Utilities.Extensions
{
    public static class IPrincipalExtension
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

        public static Guid GetMemberId(this IPrincipal currentPrincipal)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == "MemberId");

            return Guid.Parse(claim.Value);
        }

        public static string GetAccount(this IPrincipal currentPrincipal)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == "Account");

            return claim.Value;
        }

        public static string GetAccessKey(this IPrincipal currentPrincipal)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == "AccessKey");

            return claim.Value;
        }
        
    }
}
