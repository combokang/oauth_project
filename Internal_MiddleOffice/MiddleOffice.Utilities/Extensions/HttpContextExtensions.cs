using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MiddleOffice.Utilities.Extensions
{
    public static class HttpContextExtensions
    {
        public static IPAddress GetRemoteIPAddress(this HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                var header = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip;
                }
            }

            return context.Connection.RemoteIpAddress;
        }
    }
}
