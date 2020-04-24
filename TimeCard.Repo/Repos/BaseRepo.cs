using System.Collections.Generic;
using Microsoft.Data.SqlClient;

using Dapper;

namespace TimeCard.Repo
{
    public class BaseRepo
    {
        protected readonly string ConnectionString;
        public BaseRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }

        protected IEnumerable<T>QuerySp<T>(string storedProc, object parameters)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Query<T>(storedProc, parameters, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

        protected void ExecuteSp(string storedProc, object parameters)
        {
            using (var conn = GetOpenConnection())
            { 
                conn.Execute(storedProc, parameters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        protected T QuerySingleSp<T>(string storedProc, object parameters, bool orDefault = true)
        {
            using (var conn = GetOpenConnection())
            {
                if (orDefault)
                {
                    return conn.QuerySingleOrDefault<T>(storedProc, parameters, null, null, System.Data.CommandType.StoredProcedure);
                }
                else
                {
                    return conn.QuerySingle<T>(storedProc, parameters, null, null, System.Data.CommandType.StoredProcedure);
                }
            }
        }
    }
}
