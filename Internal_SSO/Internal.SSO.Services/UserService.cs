using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories;
using Internal.SSO.IServices;
using Internal.SSO.Repositories.Bases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Isopoh.Cryptography.Argon2;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace Internal.SSO.Services
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration;
        private readonly string _defaultConnectionString;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IConfiguration configuration,
            ILogger<UserService> logger)
        {
            _configuration = configuration;
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _logger = logger;
        }

        public void Add(User entity)
        {
            entity.Password = Argon2.Hash(entity.Password);

            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);
                userRepository.Add(entity);
            }
        }

        public void Delete(string id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);
                var userRoleRepository = new UserRoleRepository(context.UnitOfWork);
                var projectCodeRepository = new ProjectCodeRepository(context.UnitOfWork);

                try
                {
                    context.UnitOfWork.Begin();

                    projectCodeRepository.DeleteByUserId(id);

                    userRoleRepository.DeleteByUserId(id);

                    userRepository.Delete(id);

                    context.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    context.UnitOfWork.Rollback();
                    throw;
                }
            }

        }

        public bool Exists(string id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                return userRepository.Exists(id);
            }
        }

        public User Get(string id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                var user = userRepository.Get(id);
                user.Password = null;

                return user;
            }
        }

        public IEnumerable<UserListVM> GetAll(string id, int pageIndex, int pageSize)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                return userRepository.GetAll(id, pageIndex, pageSize);
            }
        }

        public void Unlock(string id)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                userRepository.UpdatePasswordErrorCount(id, 0);
            }
        }

        public void Update(User entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                entity.Password = null;
            }
            else
            {
                entity.Password = Argon2.Hash(entity.Password);
            }

            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                userRepository.Update(entity);
            }
        }

        public bool? Validate(string id, string password)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);
                var user = userRepository.Get(id);

                if (user == null) { return false; }

                if (user.ErrorCount > 3) { return null; }

                if (Argon2.Verify(user.Password, password))
                {
                    userRepository.UpdatePasswordErrorCount(id, 0);
                    return true;
                }

                userRepository.UpdatePasswordErrorCount(id, user.ErrorCount + 1);

                return false;
            }
        }

        public bool CheckPermission(string id, string clientId, string permissionKey)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                return userRepository.CheckPermission(id, clientId, permissionKey);
            }
        }

        public void Export(string id, Stream outStream)
        {
            using (var context = new DBContext(_defaultConnectionString))
            {
                var userRepository = new UserRepository(context.UnitOfWork);

                var data = userRepository.GetReportAll(id);

                using (var workbook = new XLWorkbook())
                {
                    var sheet1 = workbook.Worksheets.Add("sheet1");

                    var colIdx = 1;
                    int rowIdx = 1;

                    foreach (string colName in "User Id;Project Name;Role Name;Role Updater;Role Update Time".Split(';'))
                    {
                        sheet1.Cell(rowIdx, colIdx).Value = colName;

                        colIdx++;
                    }

                    rowIdx++;

                    var results = data.GroupBy(x => new { x.Id, x.ProjectName });

                    foreach (var result in results)
                    {
                        sheet1.Cell(rowIdx, 1).Value = result.Key.Id;
                        sheet1.Cell(rowIdx, 2).Value = result.Key.ProjectName;
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
