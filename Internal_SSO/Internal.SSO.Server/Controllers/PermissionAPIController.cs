using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Server.Bindings.Permissions;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/permission")]
    [ApiController]
    public class PermissionAPIController : ControllerBase
    {
        private IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public PermissionAPIController(
            IPermissionService permissionService,
            IMapper mapper)
        {
            _permissionService = permissionService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(CreatePermissionBinding input)
        {
            var permission = _mapper.Map<Permission>(input);
            permission.Creator = User.GetUserId();

            _permissionService.Add(permission);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _permissionService.Get(id);

            return Ok(result);
        }

        [HttpGet("by-projectId/{projectId}")]
        public IActionResult GetAllByProjectId(Guid projectId)
        {
            var result = _permissionService.GetAllByProjectId(projectId);

            return Ok(result);
        }

        [HttpGet("paged")]
        public IActionResult GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            var result = _permissionService.GetAll(projectId, pageIndex, pageSize);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(EditPermissionBinding input)
        {
            var permission = _mapper.Map<Permission>(input);
            permission.Updater= User.GetUserId();

            _permissionService.Update(permission);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _permissionService.Delete(id);

            return Ok();
        }
    }
}
