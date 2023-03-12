using AutoMapper;
using MiddleOffice.Entities.ViewModels.Sso;
using MiddleOffice.Entities.Models.Sso;
using MiddleOffice.Web.Bindings;
using MiddleOffice.Web.Bindings.Sso.Permissions;
using MiddleOffice.Web.Bindings.Sso.Projects;
using MiddleOffice.Web.Bindings.Sso.Roles;
using MiddleOffice.Web.Bindings.Sso.UserRoles;
using MiddleOffice.Web.Bindings.Sso.Users;
using MiddleOffice.Web.HttpClients.Sso;
using MiddleOffice.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiddleOffice.Web.Filters;
using System.IO;

namespace MiddleOffice.Web.Controllers
{
    public class SsoController : Controller
    {
        private ProjectService _projectService;
        private RoleService _roleService;
        private PermissionService _permissionService;
        private UserService _userService;
        private RolePermissionService _rolePermissionService;
        private UserRoleService _userRoleService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public SsoController(
            ProjectService projectService,
            RoleService roleService,
            PermissionService permissionService,
            UserService userService,
            RolePermissionService rolePermissionService,
            UserRoleService userRoleService,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _projectService = projectService;
            _roleService = roleService;
            _permissionService = permissionService;
            _userService = userService;
            _rolePermissionService = rolePermissionService;
            _userRoleService = userRoleService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        #region Projects

        [HttpGet]
        public IActionResult Project()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetProjectAll([FromForm] PagedProjectBinding input)
        {
            var result = await _projectService.GetPagedAllAsync(input.Name, input.PageIndex, input.Length);
            var total = result.Any() ? result.First().TotalRecords : 0;

            return Ok(new PagedResult<ProjectListVM>(result, input.Draw, total));
        }

        [HttpPost]
        public async Task<IActionResult> ExportProject(ExportProjectBinding input)
        {
            var stream = new MemoryStream();

            await _projectService.DownloadReportAsync(input.Name, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"project.xlsx"
            };
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return PartialView("_CreateProject", new CreateProjectBinding());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(CreateProjectBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _projectService.AddAsync(input.Name, input.Domain, input.CallbackUrls);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ProjectDetail(Guid id)
        {
            var project = await _projectService.GetAsync(id);

            return PartialView("_ProjectDetail", project);
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(Guid id)
        {
            var project = await _projectService.GetAsync(id);

            var model = _mapper.Map<EditProjectBinding>(project);

            return PartialView("_EditProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(EditProjectBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _projectService.EditAsync(input.Id, input.Name, input.Domain, input.CallbackUrls);

            return Ok();
        }

        [HttpGet]
        public IActionResult DeleteProject(Guid id)
        {
            return PartialView("_ModalDelete", model: null);
        }

        [HttpPost, ActionName("DeleteProject")]
        public async Task<ActionResult> DeleteUserConfirmed(Guid id)
        {
            await _projectService.DeleteAsync(id);

            return RedirectToAction("Project", "Sso", new { });
        }

        #endregion

        #region Roles

        [HttpGet]
        public async Task<IActionResult> Role(Guid projectId)
        {
            var project = await _projectService.GetAsync(projectId);

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> GetRoleAll([FromForm] PagedRoleBinding input)
        {
            var result = await _roleService.GetPagedAllAsync(input.ProjectId, input.PageIndex, input.Length);
            var total = result.Any() ? result.First().TotalRecords : 0;

            return Ok(new PagedResult<RoleListVM>(result, input.Draw, total));
        }

        [HttpGet]
        public async Task<IActionResult> RoleDetail(Guid id)
        {
            var role = await _roleService.GetAsync(id);

            var rolePermissions = await _rolePermissionService.GetAllByRoleIdAsync(id);

            ViewData["RolePermissions"] = rolePermissions;

            return PartialView("_RoleDetail", role);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole(Guid projectId)
        {
            var permissions = await _permissionService.GetAllByProjectIdAsync(projectId);

            var model = new CreateRoleBinding()
            {
                ProjectId = projectId,
                CreateRolePermissions = _mapper.Map<List<CreateRolePermissionBinding>>(permissions)
            };

            return PartialView("_CreateRole", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            var permissionIds = input.CreateRolePermissions.Where(x => x.Checked).Select(x => x.PermissionId).ToList();

            await _roleService.AddAsync(input.Name, input.Description, input.ProjectId, permissionIds);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _roleService.GetAsync(id);
            var rolePermissions = await _rolePermissionService.GetAllByRoleIdAsync(id);

            var model = _mapper.Map<EditRoleBinding>(role);
            model.EditRolePermissions = _mapper.Map<List<EditRolePermissionBinding>>(rolePermissions);

            return PartialView("_EditRole", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            var permissionIds = input.EditRolePermissions.Where(x => x.Checked).Select(x => x.PermissionId).ToList();

            await _roleService.EditAsync(input.Id, input.Name, input.Description, permissionIds);

            return Ok();
        }

        [HttpGet]
        public IActionResult DeleteRole(Guid id)
        {
            return PartialView("_ModalDelete", model: null);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(Guid id, Guid projectId)
        {
            await _roleService.DeleteAsync(id);

            return RedirectToAction("Role", "Sso", new { projectId = projectId });
        }

        #endregion

        #region Permissions

        [HttpPost]
        public async Task<IActionResult> GetPermissionAll([FromForm] PagedPermissionBinding input)
        {
            var result = await _permissionService.GetPagedAllAsync(input.ProjectId, input.PageIndex, input.Length);
            var total = result.Any() ? result.First().TotalRecords : 0;

            return Ok(new PagedResult<PermissionListVM>(result, input.Draw, total));
        }

        [HttpGet]
        public IActionResult CreatePermission(Guid projectId)
        {
            return PartialView("_CreatePermission", new CreatePermissionBinding() { ProjectId = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePermission(CreatePermissionBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _permissionService.AddAsync(input.CandidateKey, input.Name, input.Description, input.ProjectId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> EditPermission(Guid id)
        {
            var project = await _permissionService.GetAsync(id);

            var model = _mapper.Map<EditPermissionBinding>(project);

            return PartialView("_EditPermission", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPermission(EditPermissionBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _permissionService.EditAsync(input.Id, input.CandidateKey, input.Name, input.Description);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> PermissionDetail(Guid id)
        {
            var permission = await _permissionService.GetAsync(id);

            return PartialView("_PermissionDetail", permission);
        }

        [HttpGet]
        public IActionResult DeletePermission(Guid id)
        {
            return PartialView("_ModalDelete", model: null);
        }

        [HttpPost, ActionName("DeletePermission")]
        public async Task<IActionResult> DeletePermissionConfirmed(Guid id, Guid projectId)
        {
            await _permissionService.DeleteAsync(id);

            return RedirectToAction("Role", "Sso", new { projectId = projectId });
        }
        #endregion

        #region Users

        [HttpGet]
        public IActionResult User()
        {
            return View();
        }

        [HttpPost]
        [EdenredSsoAuthorize("User.Query")]
        public async Task<IActionResult> GetUserAll([FromForm] PagedUserBinding input)
        {
            var result = await _userService.GetPagedAllAsync(input.Id, input.PageIndex, input.Length);
            var total = result.Any() ? result.First().TotalRecords : 0;

            return Ok(new PagedResult<UserListVM>(result, input.Draw, total));
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return PartialView("_CreateUser", new CreateUserBinding());
        }

        [HttpPost]
        public async Task<IActionResult> ExportUserRole(ExportUserRoleBinding input)
        {
            var stream = new MemoryStream();

            await _userService.DownloadReportAsync(input.Id, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"user_project_role.xlsx"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserBinding input)
        {
            var IfExists = await _userService.ExistsAsync(input.Id);

            if (IfExists)
            {
                ModelState.AddModelError(nameof(input.Id), "此帳號已存在");

                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }
            await _userService.AddAsync(input.Id, input.Password, input.Name, input.Email);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UserDetail(string id)
        {
            var user = await _userService.GetAsync(id);

            return PartialView("_UserDetail", user);
        }

        [HttpGet]
        [EdenredSsoAuthorize("User.Edit")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userService.GetAsync(id);

            var model = _mapper.Map<EditUserBinding>(user);

            return PartialView("_EditUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _userService.EditAsync(input.Id, input.Password, input.Name, input.Email);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(UnlockUserBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            await _userService.UnlockAsync(input.Id);

            return Ok();
        }

        [HttpGet]
        public IActionResult DeleteUser(string id)
        {
            return PartialView("_ModalDelete", model: null);
        }

        [HttpPost, ActionName("DeleteUser")]

        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            await _userService.DeleteAsync(id);

            return RedirectToAction("User", "Sso", new { });
        }

        #endregion

        #region UserRole

        [HttpGet]
        public async Task<IActionResult> UserRole(string userId)
        {
            var userRoles = await _userRoleService.GetAllAsync(userId);
            var user = await _userService.GetAsync(userId);

            var projects = await _projectService.GetAllAsync();

            ViewData["Projects"] = new SelectList(projects, nameof(Entities.Models.Sso.Project.Id), nameof(Entities.Models.Sso.Project.Name));
            ViewData["User"] = user;

            return View(userRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRole(string userId, Guid projectId)
        {
            var user = await _userService.GetAsync(userId);
            var userRoles = await _userRoleService.GetAllAsync(userId);

            var model = new EditUserRoleBinding();
            model.UserId = userId;
            model.UserName = user.Name;
            model.UserRoles = _mapper.Map<List<UserRoleBinding>>(userRoles);

            return PartialView("_EditUserRole", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRole(EditUserRoleBinding input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseVM()
                {
                    FormErrors = ModelState.Select(kvp => new FormError() { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList()
                });
            }

            var userRoles = input.UserRoles.Where(x => x.Checked).Select(x => new UserRole() { RoleId = x.RoleId, UserId = input.UserId }).ToList();

            await _userRoleService.EditAsync(input.UserId, userRoles);

            return Ok();
        }

        #endregion
    }
}
