using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Internal.SSO.Repositories.Bases
{
    public class DBContext : IDisposable
    {
        private SqlConnection _connection;

        private DBContext() { }

        public DBContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            UnitOfWork = new UnitOfWork(_connection);
        }

        public UnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}
