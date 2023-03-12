using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Repositories.Bases;
using Internal.SSO.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Internal.SSO.Entities.ViewModels;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace Internal.SSO.Services
{
    public class ProjectService : IProjectService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            IConfiguration configuration,
            ILogger<ProjectService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public void Add(Project entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                entity.ClientId = Guid.NewGuid().ToString("N"); //no dashed

                RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[32];
                cryptoRandomDataGenerator.GetBytes(buffer);
                entity.ClientSecret = Convert.ToBase64String(buffer);

                projectRepository.Add(entity);
            }
        }

        public void Delete(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var rolePermissionRepository = new RolePermissionRepository(context.UnitOfWork);
                var userRoleRepository = new UserRoleRepository(context.UnitOfWork);
                var projectCodeRepository = new ProjectCodeRepository(context.UnitOfWork);
                var projectRepository = new ProjectRepository(context.UnitOfWork);
                var permissionRepository = new PermissionRepository(context.UnitOfWork);
                var roleRepository = new RoleRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    projectCodeRepository.DeleteByProjectId(id);

                    rolePermissionRepository.DeleteByProjectId(id);

                    userRoleRepository.DeleteByProjectId(id);

                    permissionRepository.DeleteByProjectId(id);

                    roleRepository.DeleteByProjectId(id);

                    projectRepository.Delete(id);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
            }
        }

        public Project Get(Guid id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                return projectRepository.Get(id);
            }
        }

        public IEnumerable<Project> GetAll()
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                return projectRepository.GetAll();
            }
        }

        public IEnumerable<ProjectListVM> GetAll(string name, int pageIndex, int pageSize)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                return projectRepository.GetAll(name, pageIndex, pageSize);
            }
        }

        public IEnumerable<ProjectInfoListVM> GetInfoAll()
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                return projectRepository.GetInfoAll();
            }
        }

        public void Update(Project entity)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                projectRepository.Update(entity);
            }
        }

        public Project GetByClientId(string clientId)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                return projectRepository.GetByClientId(clientId);
            }
        }

        public void Export(string name, Stream outStream)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var projectRepository = new ProjectRepository(context.UnitOfWork);

                var data = projectRepository.GetReportAll(name);

                using (var workbook = new XLWorkbook())
                {
                    var sheet1 = workbook.Worksheets.Add("sheet1");

                    var colIdx = 1;
                    int rowIdx = 1;

                    foreach (string colName in "Project Name;User Id;Role Name;Role Updater;Role Update Time".Split(';'))
                    {
                        sheet1.Cell(rowIdx, colIdx).Value = colName;

                        colIdx++;
                    }

                    rowIdx++;

                    var results = data.GroupBy(x => new { x.Name, x.UserId });

                    foreach (var result in results)
                    {
                        sheet1.Cell(rowIdx, 1).Value = result.Key.Name;
                        sheet1.Cell(rowIdx, 2).Value = result.Key.UserId;
                        sheet1.Cell(rowIdx, 3).Value = string.Join(',', result.Select(x => x.RoleName));
                        sheet1.Cell(rowIdx, 4).Value = result.FirstOrDefault().UserRoleCreator;
                        sheet1.Cell(rowIdx, 5).Value = result.FirstOrDefault().UserRoleCreateTime?.ToString("yyyy/MM/dd HH:mm");

                        rowIdx++;
                    }

                    workbook.SaveAs(outStream);
                }
            }
        }
    }
}
