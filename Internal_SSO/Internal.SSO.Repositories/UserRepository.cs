using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class UserRepository : BaseRepository, IGenericRepository<User, string>
    {
        private readonly UnitOfWork _unitOfWork;

        public UserRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Add(User entity)
        {
            var sql = @"
INSERT INTO [dbo].[User] ([Id] , [Password] , [Name] , [Email] , [AdGroupNames] , [ErrorCount] ,[Creator] , [CreateTime])
OUTPUT INSERTED.[Id]
VALUES (@Id ,@Password ,@Name ,@Email ,@AdGroupNames ,@ErrorCount ,@Creator ,[dbo].[GETDATE_TW]())";

            var result = _unitOfWork.Connection.ExecuteScalar<string>(sql, entity, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Delete(string id)
        {
            var sql = @"
DELETE FROM [dbo].[User]
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
        }

        public bool Exists(string id)
        {
            var sql = @"
SELECT CAST( CASE
    WHEN EXISTS
        (SELECT [Id]
         FROM [dbo].[User]
         WHERE [Id] = @Id) THEN 1
    ELSE 0
END AS BIT)";

            return _unitOfWork.Connection.QuerySingleOrDefault<bool>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
        }

        public User Get(string id)
        {
            var sql = @"
SELECT [Id],
       [Password],
       [Name],
       [Email],
       [AdGroupNames],
       [ErrorCount],
       [Creator],
       [CreateTime],
       [Updater],
       [UpdateTime]
FROM [dbo].[User]
WHERE [Id] = @Id";

            var result = _unitOfWork.Connection.QuerySingleOrDefault<User>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserListVM> GetAll(string id, int pageIndex, int pageSize)
        {
            var query = string.Empty;
            var sql = @"
WITH TempResult AS(
    SELECT [Id],
           [Name],
           [ErrorCount]
    FROM [dbo].[User] WITH(NOLOCK)
    WHERE 1 = 1 {0}
),
TotalRecords AS(
    SELECT COUNT(*) AS TotalRecords
    FROM TempResult)
SELECT *
FROM TempResult, TotalRecords
ORDER BY [Id] ASC
OFFSET (@PageIndex-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";


            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@PageIndex", pageIndex);
            dynamicParams.Add("@PageSize", pageSize);

            if (!string.IsNullOrEmpty(id))
            {
                query = "AND [Id] LIKE @Id ";
                dynamicParams.Add("@Id", $"%{id}%");
            }

            var result = _unitOfWork.Connection.Query<UserListVM>(string.Format(sql, query), dynamicParams, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Update(User entity)
        {
            var sql = @"
UPDATE [dbo].[User]
SET [Password] = ISNULL(@Password, [Password]),
    [Name] = @Name,
    [Email] = @Email ,
    [Updater] = @Updater ,
    [UpdateTime] = [dbo].[GETDATE_TW]()
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, entity, transaction: _unitOfWork.Transaction);
        }

        public void UpdatePasswordErrorCount(string id, int errorCount)
        {
            var sql = @"
UPDATE [dbo].[User]
SET [ErrorCount] = @ErrorCount
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = id, ErrorCount = errorCount }, transaction: _unitOfWork.Transaction);
        }

        public bool CheckPermission(string id, string clientId, string permissionKey)
        {
            var sql = @"
SELECT CAST(CASE
				WHEN EXISTS
						(
						SELECT T4.[Id]
						FROM [dbo].[UserRole] T1 WITH(NOLOCK)
						JOIN [dbo].[Role] T2 WITH(NOLOCK) ON T2.[Id] = T1.[RoleId]
						JOIN [dbo].[RolePermission] T3 WITH(NOLOCK) ON T3.[RoleId] = T2.[Id]
						JOIN [dbo].[Permission] T4 WITH(NOLOCK) ON T4.[Id] = T3.[PermissionId]
						JOIN [dbo].[Project] T5 WITH(NOLOCK) ON T5.[Id] = T2.[ProjectId]
						WHERE T4.[CandidateKey] = @CandidateKey
						  AND T1.[UserId] = @Id
						  AND T5.[ClientId] = @ClientId
						)THEN 1
				ELSE 0
		END AS BIT)";

            return _unitOfWork.Connection.QuerySingleOrDefault<bool>(sql, new { Id = id, ClientId = clientId, CandidateKey = permissionKey }, transaction: _unitOfWork.Transaction);
        }

        public IEnumerable<UserRVM> GetReportAll(string id)
        {
            var query = string.Empty;
            var sql = @"
SELECT T1.[Id],
       T4.[Name] AS ProjectName,
       T3.[Name] AS RoleName,
       T3.[Description] AS RoleDescription,
       T2.[Creator] AS UserRoleCreator,
       T2.[CreateTime] AS UserRoleCreateTime
FROM [dbo].[User] T1 WITH(NOLOCK)
LEFT JOIN [dbo].[UserRole] T2 WITH(NOLOCK) ON T2.[UserId] = T1.[Id]
LEFT JOIN [dbo].[Role] T3 WITH(NOLOCK) ON T3.[Id] = T2.[RoleId]
LEFT JOIN [dbo].[Project] T4 WITH(NOLOCK) ON T4.[Id] = T3.[ProjectId]
WHERE 1=1 {0}
ORDER BY T1.[Id] ASC,
         T4.[Name] ASC,
         T3.[Name] ASC";

            var dynamicParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(id))
            {
                query = "AND T1.[Id] LIKE @Id ";
                dynamicParams.Add("@Id", $"%{id}%");
            }

            return _unitOfWork.Connection.Query<UserRVM>(string.Format(sql, query), dynamicParams, transaction: _unitOfWork.Transaction);
        }
    }
}
