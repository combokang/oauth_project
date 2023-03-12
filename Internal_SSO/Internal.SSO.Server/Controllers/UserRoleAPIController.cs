using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Server.Bindings.UserRoles;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/user-role")]
    [ApiController]
    public class UserRoleAPIController : ControllerBase
    {
        private IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public UserRoleAPIController(
            IUserRoleService userRoleService,
            IMapper mapper)
        {
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult GetAll(string userId)
        {
            var result = _userRoleService.GetAll(userId);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(EditUserRoleBinding input)
        {
            var userId = User.GetUserId();
            var userRoles = _mapper.Map<List<UserRole>>(input.UserRoles);

            userRoles.ForEach(x => { x.Creator = userId; });

            _userRoleService.Update(input.UserId, userRoles);

            return Ok();
        }
    }
}
