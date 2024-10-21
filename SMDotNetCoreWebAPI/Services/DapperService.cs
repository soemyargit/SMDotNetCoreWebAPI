using Dapper;
using SMDotNetCoreWebAPI.Configurations;
using System.Data;
using System.Data.SqlClient;


namespace SMDotNetCoreWebAPI.Services
{
    public class DapperService
    {
        
        #region Query Async
        public async Task<List<T>> QueryAsync<T>(string query,object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using IDbConnection dbCon = GetSqlConnection();
                var lst = await dbCon.QueryAsync<T>(query, parameters,commandType: commandType);
                return lst.ToList();

            }
            catch (Exception ex) { throw; }

        }
        #endregion

        #region Query First Or Default Async

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string query,object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try 
            { 
                using IDbConnection dbConnection = GetSqlConnection();
                var item = await dbConnection.QueryFirstOrDefaultAsync<T>(query, parameters, commandType: commandType); 
                return item;
                
            }
            catch(Exception ex) { throw; }  
        }

        #endregion

        #region Execute Query Async

        public async Task<int> ExecuteQueryAsync(string query,object parameters)
        {
            try
            {
                using IDbConnection dbConnection = GetSqlConnection();
                int resultInt = await dbConnection.ExecuteAsync(query, parameters);
                return resultInt;
            }
            catch(Exception ex) { throw; }
        }

        #endregion

        #region GetSqlConnection
        private SqlConnection GetSqlConnection() => new(DbConfig.DbConnection); //var SqlCon = new SqlConnection(DbConfig.DbConnection); return SqlCon; }
        #endregion
    }
}
