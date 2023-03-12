using MiddleOffice.Web.HttpClients.Sso;
using MiddleOffice.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ProjectService _projectService;

        public HomeController(ILogger<HomeController> logger,ProjectService projectservice)
        {
            _logger = logger;
            _projectService = projectservice;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _projectService.GetInfoAllAsync();

            return View(result);
        }
    }
}
