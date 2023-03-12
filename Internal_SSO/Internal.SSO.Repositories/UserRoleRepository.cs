using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class UserRoleRepository : BaseRepository, IGenericRepository<UserRole, int>
    {
        private readonly UnitOfWork _unitOfWork;

        public UserRoleRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(List<UserRole> entities)
        {
            var sql = @"
INSERT INTO [dbo].[UserRole] ([UserId], [RoleId], [Creator], [CreateTime])
VALUES (@UserId,@RoleId,@Creator,[dbo].[GETDATE_TW]())";

            _unitOfWork.Connection.Execute(sql, entities, transaction: _unitOfWork.Transaction);
        }


        public UserRole Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRoleVM> GetAll(string userId)
        {
            var sql = @"
SELECT T2.[Name] AS ProjectName,
       T1.[Id] AS RoleId,
       T1.[Name] AS RoleName,
       CAST(CASE
                WHEN T3.[Id] IS NOT NULL THEN 1
                ELSE 0
            END AS BIT)AS Checked,
       T3.[Creator],
	   T3.[CreateTime]
FROM [dbo].[Role] T1
JOIN [dbo].[Project] T2 ON T2.[Id] = T1.[ProjectId]
LEFT JOIN[dbo].[UserRole] T3 ON T3.[RoleId] = T1.[Id]
AND T3.[UserId] = @UserId
ORDER BY T2.[Name] ASC, T1.[Name] ASC";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId);

            var result = _unitOfWork.Connection.Query<UserRoleVM>(sql, dynamicParams, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void DeleteByUserId(string userId)
        {
            var sql = @"
DELETE FROM [dbo].[UserRole]
WHERE [UserId] = @UserId";

            _unitOfWork.Connection.Execute(sql, new { UserId = userId }, transaction: _unitOfWork.Transaction);
        }

        public void DeleteByProjectId(Guid projectId)
        {
            var sql = @"
DELETE T1 FROM [dbo].[UserRole] T1
LEFT JOIN [dbo].[Role] AS T2 ON T1.[RoleId] = T2.[Id]
WHERE T2.[ProjectId] = @ProjectId";

            _unitOfWork.Connection.Execute(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);
        }

        public IEnumerable<UserRole> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(UserRole entity)
        {
            throw new NotImplementedException();
        }

        public int Add(UserRole entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
