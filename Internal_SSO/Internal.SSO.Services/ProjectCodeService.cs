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
    public class ProjectCodeService : IProjectCodeService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<PermissionService> _logger;

        public ProjectCodeService(
            IConfiguration configuration,
            ILogger<PermissionService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public void Add(ProjectCode entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userCodeRepository = new ProjectCodeRepository(context.UnitOfWork);

                userCodeRepository.Add(entity);
            }
        }

        public ProjectCode GetByCode(string code)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectCodeRepository = new ProjectCodeRepository(context.UnitOfWork);

                return projectCodeRepository.GetByCode(code);
            }
        }
    }
}
