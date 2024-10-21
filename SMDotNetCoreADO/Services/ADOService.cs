using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SMDotNetCoreADO.Configurations;
using SMDotNetCoreADO.Models;
using SMDotNetCoreADO.Queries;
using System.Data;
using System.Net.Quic;

namespace SMDotNetCoreADO.Services
{
    public class ADOService
    {
        private readonly IConfiguration _configuration;

        public ADOService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        #region CmdExecuteReader
        public List<BlogModel> cmdExecReader(string sqlQuery, int? id = null)
        {
            string connString = GetSqlConnectionString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                //SqlDataReader
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                List<BlogModel> lstBlogModel = new List<BlogModel>();


                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        BlogModel blog = new BlogModel();
                        blog.BlogId = Convert.ToInt32(dataReader["BlogId"]);
                        blog.BlogTitle = Convert.ToString(dataReader["BlogTitle"]);
                        blog.BlogAuthor = Convert.ToString(dataReader["BlogAuthor"]);
                        blog.BlogContent = Convert.ToString(dataReader["BlogContent"]);
                        lstBlogModel.Add(blog);
                    }
                }
                connection.Close();
                return lstBlogModel;
            }

        }
        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string sqlQuery, string? strType = null, BlogRequestModel? blogRequestModel = null, int? id = null)
        {
            string conString = GetSqlConnectionString();
            string sqlQueryString = string.Empty;
            int iResult = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand(sqlQueryString, con))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    if (id != null) { command.Parameters.AddWithValue("@BlogId", SqlDbType.NVarChar).Value = id; }
                    if (strType != "D")
                    {
                        command.Parameters.AddWithValue("@BlogTitle", SqlDbType.NVarChar).Value = blogRequestModel.BlogTitle;
                        command.Parameters.AddWithValue("@BlogAuthor", SqlDbType.NVarChar).Value = blogRequestModel.BlogAuthor;
                        command.Parameters.AddWithValue("@BlogContent", SqlDbType.NVarChar).Value = blogRequestModel.BlogContent;
                    }
                    command.CommandText = sqlQuery;
                    con.Open();
                    iResult = command.ExecuteNonQuery();
                    con.Close();
                }
            }

            return iResult;
        }

        #endregion

        #region GetSqlConnection
        private SqlConnection GetSqlConnection() => new(DbConfig.DbConnection); //var SqlCon = new SqlConnection(DbConfig.DbConnection); return SqlCon; }

        private string GetSqlConnectionString() => new(DbConfig.DbConnection);


        #endregion
    }
}
