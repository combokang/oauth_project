using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Repositories.Bases;
using Internal.SSO.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Internal.SSO.Entities.ViewModels;
using System.Linq;

namespace Internal.SSO.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<RolePermissionService> _logger;

        public RolePermissionService(
            IConfiguration configuration,
            ILogger<RolePermissionService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public IEnumerable<RolePermissionVM> GetAllByRoleId(Guid roleId)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);
                var roleRepository = new RoleRepository(context.UnitOfWork);
                var permissionRepository = new PermissionRepository(context.UnitOfWork);

                var role = roleRepository.Get(roleId);
                var rolePermissions= rolePermissionRepository.GetAll(roleId);
                var permsiions = permissionRepository.GetAllByProjectId(role.ProjectId);

                var result = permsiions.Select(x => new RolePermissionVM()
                {
                    Checked = rolePermissions.Any(y => y.PermissionId == x.Id),
                    PermissionId = x.Id,
                    PermissionCandidateKey = x.CandidateKey,
                    PermissionName = x.Name,
                    PermissionDescription = x.Description
                });

                return result;
            }
        }
    }
}
