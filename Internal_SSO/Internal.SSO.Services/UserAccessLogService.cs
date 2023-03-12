using Internal.SSO.Entities.Models;
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
    public class UserAccessLogService : IUserAccessLogService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<UserAccessLogService> _logger;

        public UserAccessLogService(
            IConfiguration configuration,
            ILogger<UserAccessLogService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public void Add(UserAccessLog entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserAccessLogRepository(context.UnitOfWork);
                userRepository.Add(entity);
            }
        }
    }
}
