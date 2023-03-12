using Dapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories
{
    public class UserAccessLogRepository : BaseRepository, IGenericRepository<UserAccessLog, int>
    {
        private readonly UnitOfWork _unitOfWork;

        public UserAccessLogRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(UserAccessLog entity)
        {
            var sql = @"
INSERT INTO [dbo].[UserAccessLog] ([ActionId] , [Memo] , [UserId] , [CreateTime]) OUTPUT INSERTED.[Id]
VALUES (@ActionId,@Memo,@UserId,[dbo].[GETDATE_TW]())";

            var result = _unitOfWork.Connection.ExecuteScalar<int>(sql, entity, transaction: _unitOfWork.Transaction);

            return result;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserAccessLog Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserAccessLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(UserAccessLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
