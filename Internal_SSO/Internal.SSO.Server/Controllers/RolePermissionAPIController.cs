using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/role-permission")]
    [ApiController]
    public class RolePermissionAPIController : ControllerBase
    {
        private IRolePermissionService _rolePermissionService;
        private readonly IMapper _mapper;

        public RolePermissionAPIController(
            IRolePermissionService rolePermissionService,
            IMapper mapper)
        {
            _rolePermissionService = rolePermissionService;
            _mapper = mapper;
        }

        [HttpGet("by-roleId/{roleId}")]
        public IActionResult GetAllByRoleId(Guid roleId)
        {
            var result = _rolePermissionService.GetAllByRoleId(roleId);

            return Ok(result);
        }
    }
}
