using AutoMapper;
using Internal.SSO.Entities.Enums.OAuths;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Server.Bindings.Users;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private IUserService _userService;
        private IUserAccessLogService _userAccessLogService;
        private readonly IMapper _mapper;

        public UserAPIController(
            IUserService userService,
            IUserAccessLogService userAccessLogService,
            IMapper mapper)
        {
            _userService = userService;
            _userAccessLogService = userAccessLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(CreateUserBinding input)
        {
            var user = _mapper.Map<User>(input);
            user.Creator = User.GetUserId();

            _userService.Add(user);

            return Ok();
        }

        [HttpPost("unlock")]
        public IActionResult Unlock(UnlockUserBinding input)
        {
            var user = _mapper.Map<User>(input);
            _userService.Unlock(user.Id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);

            return Ok();
        }

        [HttpGet("exists")]
        public IActionResult Exists(string id)
        {
            var result = _userService.Exists(id);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _userService.Get(id);

            return Ok(result);
        }

        [HttpGet("paged")]
        public IActionResult GetAll(string id, int pageIndex, int pageSize)
        {
            var result = _userService.GetAll(id, pageIndex, pageSize);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(EditUserBinding input)
        {
            var user = _mapper.Map<User>(input);
            user.Updater = User.GetUserId();

            _userService.Update(user);

            return Ok();
        }

        [HttpPost("export")]
        public IActionResult ExportExcel(ExportUserBinding input)
        {
            var stream = new MemoryStream();

            _userService.Export(input.Id, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"export.xlsx");
        }

        [HttpPost("permission")]
        public IActionResult ValidatePermission(ValidateUserPermissionBinding input)
        {
            var result = _userService.CheckPermission(User.GetUserId(), input.ClientId, input.PermissionKey);

            if (!result) { return StatusCode((int)HttpStatusCode.Forbidden); }

            _userAccessLogService.Add(new UserAccessLog()
            {
                ActionId = (int)UserAccessLogAction.GetPermission,
                Memo = $"permission-key : {input.PermissionKey} , client-id : {input.ClientId}",
                UserId = User.GetUserId(),
            });

            return Ok();
        }
    }
}
