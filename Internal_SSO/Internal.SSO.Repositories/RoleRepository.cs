using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Entities.ViewModels;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class RoleRepository : BaseRepository, IGenericRepository<Role, Guid>
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Guid Add(Role entity)
        {
            var sql = @"
INSERT INTO [dbo].[Role] ([Id] , [Name] , [Description] , [ProjectId] , [Creator] , [CreateTime])
OUTPUT INSERTED.[Id]
VALUES (NEWID() ,@Name ,@Description ,@ProjectId ,@Creator ,[dbo].[GETDATE_TW]())";

            var result = _unitOfWork.Connection.ExecuteScalar<Guid>(sql, entity, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Delete(Guid id)
        {
            var sql = @"
DELETE FROM [dbo].[Role]
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
        }
        public void DeleteByProjectId(Guid projectId)
        {
            var sql = @"
DELETE FROM [dbo].[Role]
WHERE [ProjectId] = @ProjectId";

            _unitOfWork.Connection.Execute(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);
        }

        public Role Get(Guid id)
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Description] ,
       [ProjectId] ,
       [Creator] ,
       [CreateTime] ,
       [Updater] ,
       [UpdateTime]
FROM [dbo].[Role]
WHERE [Id] = @Id";

            var result = _unitOfWork.Connection.QuerySingleOrDefault<Role>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<RoleListVM> GetAll(Guid projectId, int pageIndex, int pageSize)
        {
            var sql = @"
SELECT [Id] ,
       [Name] ,
       [Description] ,
       [ProjectId]
FROM [dbo].[Role]
WHERE [ProjectId]=@ProjectId
ORDER BY [Name] ASC";

            var result = _unitOfWork.Connection.Query<RoleListVM>(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);

            return result;
        }


        public void Update(Role entity)
        {
            var sql = @"
UPDATE [dbo].[Role]
SET [Name] = @Name ,
    [Description] = @Description ,
    [Updater] = @Updater ,
    [UpdateTime] = [dbo].[GETDATE_TW]()
WHERE [Id] = @Id";

            _unitOfWork.Connection.Execute(sql, entity, transaction: _unitOfWork.Transaction);
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
