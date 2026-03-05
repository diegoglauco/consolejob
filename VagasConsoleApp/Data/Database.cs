using Microsoft.Data.SqlClient;

namespace VagasConsoleApp.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Execute(string sql, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            cmd.ExecuteNonQuery();
        }
    }
}