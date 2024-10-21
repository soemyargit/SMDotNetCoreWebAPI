using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace SMDotNetCoreADO.Services
{
    public class AdoDotNetService
    {
        private SqlConnection GetSqlConnection() => new("");

        public async Task<List<T>> QueryAsync<T>(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                SqlConnection connection = GetSqlConnection();
                await connection.OpenAsync();

                SqlCommand command = new(query, connection) { CommandType = commandType };
                if (parameters is not null)
                {
                    command.Parameters.AddRange(parameters);
                }

                SqlDataAdapter adapter = new(command);
                DataTable dt = new();
                adapter.Fill(dt);
                await connection.CloseAsync();

                string jsonStr = JsonConvert.SerializeObject(dt);
                var lst = JsonConvert.DeserializeObject<List<T>>(jsonStr)!;

                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> QueryFirstOrDefaultAsync(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                SqlConnection connection = GetSqlConnection();
                await connection.OpenAsync();

                SqlCommand command = new(query, connection) { CommandType = commandType };
                if (parameters is not null)
                {
                    command.Parameters.AddRange(parameters);
                }

                SqlDataAdapter adapter = new(command);
                DataTable dt = new();
                adapter.Fill(dt);
                await connection.CloseAsync();

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
