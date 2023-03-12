using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Server.Bindings.Roles;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleAPIController : ControllerBase
    {
        private IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleAPIController(
            IRoleService roleService,
            IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(CreateRoleBinding input)
        {
            var role = _mapper.Map<Role>(input);
            role.Creator = User.GetUserId();

            var rolePermissions = input.PermissionIds.Select(x => new RolePermission() { RoleId = role.Id, PermissionId = x }).ToList();

            _roleService.Add(role, rolePermissions);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _roleService.Get(id);

            return Ok(result);
        }

        [HttpGet("paged")]
        public IActionResult GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            var result = _roleService.GetAll(projectId, pageIndex, pageSize);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(EditRoleBinding input)
        {
            var role = _mapper.Map<Role>(input);
            role.Updater = User.GetUserId();

            var rolePermissions = input.PermissionIds.Select(x => new RolePermission() { RoleId = role.Id, PermissionId = x }).ToList();

            _roleService.Update(role, rolePermissions);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _roleService.Delete(id);

            return Ok();
        }
    }
}
