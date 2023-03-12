using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Repositories.Bases;
using Internal.SSO.Repositories;
using Internal.SSO.Entities.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Services
{
    public class PermissionService : IPermissionService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            IConfiguration configuration,
            ILogger<PermissionService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public IEnumerable<PermissionListVM> GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);

                return permissionRepository.GetAll(projectId, pageIndex, pageSize);
            }
        }

        public IEnumerable<Permission> GetAllByProjectId(Guid projectId)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);

                return permissionRepository.GetAllByProjectId(projectId);
            }
        }

        public void Add(Permission entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);
                permissionRepository.Add(entity);
            }
        }

        public void Delete(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);
                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    rolePermissionRepository.DeleteByPermissionId(id);
                    permissionRepository.Delete(id);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
            }
        }

        public Permission Get(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);

                return permissionRepository.Get(id);
            }
        }

        public void Update(Permission entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var permissionRepository = new PermissionRepository(context.UnitOfWork);

                permissionRepository.Update(entity);
            }
        }
    }
}
