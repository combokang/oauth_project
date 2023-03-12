using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.IServices;
using Internal.SSO.Repositories;
using Internal.SSO.Repositories.Bases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Services
{
    public class UserRoleService : IUserRoleService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<UserRoleService> _logger;

        public UserRoleService(
            IConfiguration configuration,
            ILogger<UserRoleService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public IEnumerable<UserRoleVM> GetAll(string userId)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRoleRepository = new UserRoleRepository(context.UnitOfWork);

                return userRoleRepository.GetAll(userId);
            }
        }

        public void Update(string userId, List<UserRole> entities)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRoleRepository = new UserRoleRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    userRoleRepository.DeleteByUserId(userId);

                    userRoleRepository.Add(entities);

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
