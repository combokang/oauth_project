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

namespace Internal.SSO.Services
{
    public class RoleService : IRoleService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            IConfiguration configuration,
            ILogger<RoleService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public void Add(Role entity, List<RolePermission> rolePermissions)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var roleRepository = new RoleRepository(context.UnitOfWork);

                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    var roleId = roleRepository.Add(entity);

                    rolePermissions.ForEach(x => { x.RoleId = roleId; });

                    rolePermissionRepository.Add(rolePermissions);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var roleRepository = new RoleRepository(context.UnitOfWork);
                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    rolePermissionRepository.DeleteByRoleId(id);
                    roleRepository.Delete(id);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
                
            }
        }

        public Role Get(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var roleRepository = new RoleRepository(context.UnitOfWork);

                return roleRepository.Get(id);
            }
        }

        public IEnumerable<RoleListVM> GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var roleRepository = new RoleRepository(context.UnitOfWork);

                return roleRepository.GetAll(projectId, pageIndex, pageSize);
            }
        }

        public void Update(Role entity, List<RolePermission> rolePermissions)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var roleRepository = new RoleRepository(context.UnitOfWork);

                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    rolePermissionRepository.DeleteByRoleId(entity.Id);

                    rolePermissionRepository.Add(rolePermissions);

                    roleRepository.Update(entity);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
