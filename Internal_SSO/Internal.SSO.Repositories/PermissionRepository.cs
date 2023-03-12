using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class PermissionRepository : BaseRepository, IGenericRepository<Permission, Guid>
    {
        private readonly UnitOfWork _unitOfWork;

        public PermissionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PermissionListVM> GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            var sql = @"
WITH TempResult AS(
    SELECT [Id],
           [CandidateKey],
           [Name],
           [Description]
    FROM [dbo].[Permission] WITH(NOLOCK)
    WHERE [ProjectId] = @ProjectId 
),   
TotalRecords AS(
    SELECT COUNT(*) AS TotalRecords
   FROM TempResult)
SELECT *
FROM TempResult, TotalRecords
ORDER BY [Name] ASC
OFFSET (@PageIndex-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@ProjectId", projectId);
            dynamicParams.Add("@PageIndex", pageIndex);
            dynamicParams.Add("@PageSize", pageSize);

            var result = _unitOfWork.Connection.Query<PermissionListVM>(sql, dynamicParams, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<Permission> GetAllByProjectId(Guid projectId)
        {
            var sql = @"
SELECT [Id],
       [CandidateKey],
       [Name],
       [Description],
       [ProjectId],
       [Creator],
       [CreateTime],
       [Updater],
       [UpdateTime]
FROM [dbo].[Permission]
WHERE [ProjectId] = @ProjectId
ORDER BY [CandidateKey] ASC";

            var result = _unitOfWork.Connection.Query<Permission>(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public Guid Add(Permission entity)
        {
            var sql = @"
INSERT INTO [dbo].[Permission] ([Id], [CandidateKey], [Name], [Description], [ProjectId], [Creator], [CreateTime]) OUTPUT INSERTED.[Id]
VALUES (NEWID(),@CandidateKey,@Name,@Description,@ProjectId,@Creator,[dbo].[GETDATE_TW]())";

            var result = _unitOfWork.Connection.ExecuteScalar<Guid>(sql, entity, transaction: _unitOfWork.Transaction);

            return result;
        }

        public Permission Get(Guid id)
        {
            var sql = @"
SELECT [Id],
       [CandidateKey],
       [Name],
       [Description],
       [ProjectId],
       [Creator],
       [CreateTime],
       [Updater],
       [UpdateTime]
FROM [dbo].[Permission]
WHERE [Id] = @Id";

            var result = _unitOfWork.Connection.QuerySingleOrDefault<Permission>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Update(Permission entity)
        {
            var sql = @"
UPDATE [dbo].[Permission]
SET [CandidateKey] = @CandidateKey,
    [Name] = @Name,
    [Description] = @Description,
    [Updater] = @Updater,
    [UpdateTime] = [dbo].[GETDATE_TW]()
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, entity, transaction: _unitOfWork.Transaction);
        }
        public IEnumerable<Permission> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            var sql = @"
DELETE FROM [dbo].[Permission]
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
        }

        public void DeleteByProjectId(Guid projectId)
        {
            var sql = @"
DELETE FROM [dbo].[Permission]
WHERE [ProjectId] = @ProjectId";

            _unitOfWork.Connection.Execute(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);
        }
    }
}
