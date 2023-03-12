using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class RolePermissionRepository : BaseRepository, IGenericRepository<RolePermission, Guid>
    {
        private readonly UnitOfWork _unitOfWork;

        public RolePermissionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(List<RolePermission> entities)
        {
            var sql = @"
INSERT INTO [dbo].[RolePermission] ([Id], [RoleId], [PermissionId])
VALUES (NEWID(),@RoleId,@PermissionId)";

            _unitOfWork.Connection.Execute(sql, entities, transaction: _unitOfWork.Transaction);
        }

        public void DeleteByRoleId(Guid roleId)
        {
            var sql = @"
DELETE FROM [dbo].[RolePermission]
WHERE [RoleId] = @RoleId";

            _unitOfWork.Connection.Execute(sql, new { RoleId = roleId }, transaction: _unitOfWork.Transaction);
        }

        public void DeleteByPermissionId(Guid permissionId)
        {
            var sql = @"
DELETE FROM [dbo].[RolePermission]
WHERE [PermissionId] = @PermissionId";

            _unitOfWork.Connection.Execute(sql, new { PermissionId = permissionId }, transaction: _unitOfWork.Transaction);
        }
        public void DeleteByProjectId(Guid projectId)
        {
            var sql = @"
DELETE T1 FROM [dbo].[RolePermission] T1
LEFT JOIN [dbo].[Role] AS T2 ON T1.[RoleId] = T2.[Id]
WHERE T2.[ProjectId] = @ProjectId";

            _unitOfWork.Connection.Execute(sql, new { ProjectId = projectId }, transaction: _unitOfWork.Transaction);
        }

        public Guid Add(RolePermission entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public RolePermission Get(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RolePermission> GetAll(Guid roleId)
        {
            var sql = @"
SELECT [Id],
       [RoleId],
       [PermissionId]
FROM [dbo].[RolePermission]
WHERE [RoleId] = @RoleId";

            var result = _unitOfWork.Connection.Query<RolePermission>(sql, new { RoleId = roleId }, transaction: _unitOfWork.Transaction);

            return result;
        }

        public IEnumerable<RolePermission> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(RolePermission entity)
        {
            throw new NotImplementedException();
        }
    }
}

