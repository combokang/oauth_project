using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class ProjectCodeRepository : BaseRepository, IGenericRepository<ProjectCode, Guid>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProjectCodeRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Guid Add(ProjectCode entity)
        {
            var sql = @"
INSERT INTO [dbo].[ProjectCode] ([Id] , [ProjectId] , [UserId] , [Code] , [Expires] , [CreateTime])
OUTPUT INSERTED.[Id]
VALUES (NEWID() ,@ProjectId ,@UserId ,@Code ,@Expires ,[dbo].[GETDATE_TW]())";

            return _unitOfWork.Connection.ExecuteScalar<Guid>(sql, entity, transaction: _unitOfWork.Transaction);
        }
        public void DeleteByUserId(string userId)
        {
            var sql = @"
DELETE FROM [dbo].[ProjectCode]
WHERE [UserId] = @UserId";

            _unitOfWork.Connection.Execute(sql, new { UserId = userId }, transaction: _unitOfWork.Transaction);
        }
        public void DeleteByProjectId(Guid projectId)
        {
            var sql = @"
DELETE FROM [dbo].[ProjectCode]
WHERE [ProjectId] = @ProjectId";

            _unitOfWork.Connection.Execute(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ProjectCode Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ProjectCode GetByCode(string code)
        {
            var sql = @"
SELECT [Id] ,
       [ProjectId] ,
       [UserId] ,
       [Code] ,
       [Expires] ,
       [CreateTime]
FROM [dbo].[ProjectCode]
WHERE [Code] = @Code";

            return _unitOfWork.Connection.QuerySingleOrDefault<ProjectCode>(sql, new { Code = code }, transaction: _unitOfWork.Transaction);
        }

        public ProjectCode GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectCode> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ProjectCode entity)
        {
            throw new NotImplementedException();
        }
    }
}
