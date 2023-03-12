using Internal.SSO.Entities.Enums.OAuths;
using Internal.SSO.Server.Bindings.OAuths;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Internal.SSO.Server.Controllers
{
    public class OAuthController : Controller
    {
        private IProjectService _projectService;
        private IUserService _userService;
        private IProjectCodeService _projectCodeService;
        private IUserAccessLogService _userAccessLogService;
        private IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OAuthController(
            IProjectService projectService,
            IUserService userService,
            IProjectCodeService projectCodeService,
            IUserAccessLogService userAccessLogService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _projectService = projectService;
            _userService = userService;
            _projectCodeService = projectCodeService;
            _userAccessLogService = userAccessLogService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Authorize(
            string response_type,      
            string client_id,       
            string redirect_uri,       
            string scope,         
            string state)
        {
            if (response_type != "code") { return RedirectToAction("NotFoundPage", "Error"); }
            
            var project = _projectService.GetByClientId(client_id);

            if (project == null) { return RedirectToAction("NotFoundPage", "Error"); }

            var redirectUris = project.CallbackUrls?.Split(';');
            if (redirectUris == null || !redirectUris.Contains(redirect_uri)) { return RedirectToAction("NotFoundPage", "Error"); }

            var loginTypes = Enum<LoginType>.List.ToDictionary(x => (int)x, x => x.GetDescription());

            ViewData["LoginTypes"] = new SelectList(loginTypes, "Key", "Value");

            return View(new AuthorizeBinding() { ProjectId = project.Id, RedirectUri = redirect_uri, State = state });
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authorize(AuthorizeBinding input)
        {
            var loginTypes = Enum<LoginType>.List.ToDictionary(x => (int)x, x => x.GetDescription());

            ViewData["LoginTypes"] = new SelectList(loginTypes, "Key", "Value");

            if (!ModelState.IsValid) { return View(input); }

            if (input.TypeId == (int)LoginType.Custom)
            {
                var pass = _userService.Validate(input.Username, input.Password);

                if (pass == null)
                {
                    ModelState.AddModelError(nameof(input.Password), "輸入錯誤超過三次已鎖定，請聯絡管理員。");
                    return View(input);
                }
                else if (pass.HasValue && !pass.Value)
                {
                    ModelState.AddModelError(nameof(input.Password), "帳號或密碼有誤");
                    return View(input);
                }
            }

            if (input.TypeId == (int)LoginType.AD)
            {
                var userName = $"{input.Username}@mail.com";
                using var principalContext = new PrincipalContext(ContextType.Domain, "name.asnet.accorservices.net", userName, input.Password);

                if (!principalContext.ValidateCredentials(userName, input.Password))
                {
                    ModelState.AddModelError(nameof(input.Password), "帳號或密碼有誤");
                    return View(input);
                }

                if (!_userService.Exists(input.Username))
                {
                    var userInfo = UserPrincipal.FindByIdentity(principalContext, userName);

                    _userService.Add(new User()
                    {
                        Id = input.Username.ToLower(),
                        Password = Guid.NewGuid().ToString("N"),
                        Name = userInfo.GivenName,
                        Email = $"{input.Username}@mail.com",
                        AdGroupNames = string.Empty,
                        ErrorCount = 0,
                        Creator = "sys",
                    });
                }
            }

            var code = Guid.NewGuid().ToString("N");

            _userAccessLogService.Add(new UserAccessLog()
            {
                ActionId = (int)UserAccessLogAction.Login,
                Memo = String.Empty,
                UserId = input.Username.ToLower(),
            }) ;

            _projectCodeService.Add(new ProjectCode()
            {
                ProjectId = input.ProjectId,
                UserId = input.Username.ToLower(),
                Code = code,
                Expires = DateTime.Now.AddDays(1)
            });

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", input.State);

            return Redirect($"{input.RedirectUri}{query.ToString()}");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Token(
            string grant_type,
            string code,
            string redirect_uri,
            string client_id,
            string client_secret,
            string refresh_token)
        {
            var project = _projectService.GetByClientId(client_id);

            if (project == null || project.ClientSecret != client_secret) { return BadRequest(); }

            var redirectUris = project.CallbackUrls?.Split(';');
            if (redirectUris == null || !redirectUris.Contains(redirect_uri)) { return BadRequest(); }

            var projectCode = _projectCodeService.GetByCode(code);

            if (projectCode == null || projectCode.Expires < DateTime.Now) { return BadRequest(); }

            var user = _userService.Get(projectCode.UserId);

            var claims = new[]
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var secretBytes = Encoding.UTF8.GetBytes(_configuration["Constants:Secret"]);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _configuration["Constants:Issuer"],
                _configuration["Constants:Audience"],
                claims,
                notBefore: DateTime.Now,
                expires: grant_type == "refresh_token"
                    ? DateTime.Now.AddDays(1)
                    : DateTime.Now.AddDays(1),
                signingCredentials);

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "Sso",
                refresh_token = "RefreshTokenValue"
            };

            var responseJson = JsonSerializer.Serialize(responseObject);

            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }

        [Authorize]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
