using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Internal.SSO.Repositories.Bases
{
    public class UnitOfWork
    {
        public UnitOfWork(SqlConnection connection)
        {
            Id = Guid.NewGuid();
            Connection = connection;
        }

        public Guid Id { get; }

        public SqlConnection Connection { get; }

        public SqlTransaction Transaction { get; private set; }

        public void Begin()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            Dispose();
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }

            Transaction = null;
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }

            Dispose();
        }
    }
}
