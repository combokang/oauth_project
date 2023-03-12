using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class ProjectRepository : BaseRepository, IGenericRepository<Project, Guid>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProjectRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Guid Add(Project entity)
        {
            var sql = @"
INSERT INTO [dbo].[Project] ([Id] , [Name] , [Domain] , [CallbackUrls] , [ClientId] , [ClientSecret] , [IsDeleted] , [Creator] , [CreateTime])
OUTPUT INSERTED.[Id]
VALUES (NEWID() ,@Name ,@Domain ,@CallbackUrls ,@ClientId ,@ClientSecret ,@IsDeleted ,@Creator ,[dbo].[GETDATE_TW]())";

            var result = _unitOfWork.Connection.ExecuteScalar<Guid>(sql, entity, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Delete(Guid id)
        {
            var sql = @"
DELETE FROM [dbo].[Project]
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
        }

        public Project Get(Guid id)
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Domain] ,
       [CallbackUrls] ,
       [ClientId] ,
       [ClientSecret] ,
       [IsDeleted] ,
       [Creator] ,
       [CreateTime] ,
       [Updater] ,
       [UpdateTime]
FROM [dbo].[Project]
WHERE [Id] = @Id";

            var result = _unitOfWork.Connection.QuerySingleOrDefault<Project>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public Project GetByClientId(string clientId)
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Domain] ,
       [CallbackUrls] ,
       [ClientId] ,
       [ClientSecret] ,
       [IsDeleted] ,
       [Creator] ,
       [CreateTime] ,
       [Updater] ,
       [UpdateTime]
FROM [dbo].[Project]
WHERE [ClientId] = @ClientId";

            var result = _unitOfWork.Connection.QuerySingleOrDefault<Project>(sql, new { ClientId = clientId }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<Project> GetAll()
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Domain] ,
       [CallbackUrls] ,
       [ClientId] ,
       [ClientSecret] ,
       [IsDeleted] ,
       [Creator] ,
       [CreateTime] ,
       [Updater] ,
       [UpdateTime]
FROM [dbo].[Project]
ORDER BY [Name] ASC";

            var result = _unitOfWork.Connection.Query<Project>(sql, transaction: _unitOfWork.Transaction);

            return result;
        }
        public IEnumerable<ProjectInfoListVM> GetInfoAll()
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Domain]
FROM [dbo].[Project]
ORDER BY [Name] ASC";

            var result = _unitOfWork.Connection.Query<ProjectInfoListVM>(sql, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<ProjectListVM> GetAll(string name, int pageIndex, int pageSize)
        {
            var query = string.Empty;
            var sql = @"
WITH TempResult AS(
    SELECT [Id] ,
           [Name] ,
           [Domain]
    FROM [dbo].[Project]
    WHERE 1 = 1 {0}
),
TotalRecords AS (
    SELECT COUNT(*) AS TotalRecords FROM TempResult
)

SELECT *
FROM TempResult, TotalRecords
ORDER BY [Name] ASC
OFFSET (@PageIndex-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@PageIndex", pageIndex);
            dynamicParams.Add("@PageSize", pageSize);

            if (!string.IsNullOrEmpty(name))
            {
                query = "AND [Name] LIKE @Name ";
                dynamicParams.Add("@Name", $"%{name}%");
            }

            var result = _unitOfWork.Connection.Query<ProjectListVM>(string.Format(sql, query), dynamicParams, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Update(Project entity)
        {
            var sql = @"
UPDATE [dbo].[Project]
SET [Name] = @Name ,
    [Domain] = @Domain ,
    [CallbackUrls] = @CallbackUrls ,
    [Updater] = @Updater ,
    [UpdateTime] = [dbo].[GETDATE_TW]()
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, entity, transaction: _unitOfWork.Transaction);
        }

        public IEnumerable<ProjectRVM> GetReportAll(string name)
        {
            var query = string.Empty;
            var sql = @"
SELECT T1.[Name],
	   T4.[Id] AS UserId,
	   T2.[Name] AS RoleName,
	   T3.[Creator] AS UserRoleCreator,
       T3.[CreateTime] AS UserRoleCreateTime
FROM [dbo].[Project] T1 WITH(NOLOCK)
JOIN [dbo].[Role] T2 WITH(NOLOCK) ON T2.[ProjectId] = T1.[Id]
JOIN [dbo].[UserRole] T3 WITH(NOLOCK) ON T3.[RoleId] = T2.[Id]
JOIN [dbo].[User] T4 WITH(NOLOCK) ON T4.[Id] = T3.[UserId]
WHERE 1 = 1 {0}
ORDER BY T1.[Name] ASC,
         T4.[Id] ASC,
         T2.[Name] ASC";

            var dynamicParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                query = "AND T1.[Name] LIKE @Name ";
                dynamicParams.Add("@Name", $"%{name}%");
            }

            return _unitOfWork.Connection.Query<ProjectRVM>(string.Format(sql, query), dynamicParams, transaction: _unitOfWork.Transaction);
        }
    }
}
