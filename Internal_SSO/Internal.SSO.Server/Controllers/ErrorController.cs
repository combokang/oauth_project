using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult ForbiddenPage()
        {
            return View();
        }

        public IActionResult InternalServerErrorPage()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
