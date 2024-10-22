using Dapper;
using Microsoft.AspNetCore.Mvc;
using SMDotNetCoreWebAPI.Models;
using SMDotNetCoreWebAPI.Configurations;
using System.Data;
using System.Data.SqlClient;

namespace SMDotNetCoreWebAPI.Controllers;

[Route("api/Controller")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public BlogController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogAsync()
    {

        try
        {
            string query = @"SELECT BlogId, BlogTitle, BlogAuthor, BlogContent
FROM Tbl_Blog";
            //var connectionString = @"Data Source=soe\mssql; Initial Catalog=TestDb;User ID=sa; Password=sasa@123; TrustServerCertificate=True";

            //using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection"));

            var connectionString = _configuration.GetConnectionString("DbConnection");
            connectionString = connectionString.Replace("\\", @"\");

            using IDbConnection db = new SqlConnection(connectionString);
            var lst = await db.QueryAsync<BlogModel>(query);

            return Ok(lst.ToList());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogByIDAsync(int id)
    {
        try
        {
            string query = "SELECT BlogId, BlogTitle, BlogAuthor, BlogContent FROM Tbl_Blog WHERE BlogId = @BlogId";
            using IDbConnection db = new SqlConnection(DbConfig.DbConnection);
            var lst = await db.QueryFirstOrDefaultAsync<BlogModel>(query, new { BlogId = id });

            return Ok(lst);
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogAsync([FromBody] BlogRequestModel blogRequest)
    {
        try
        {
            string query = @"INSERT INTO Tbl_Blog (BlogTitle, BlogAuthor, BlogContent)
VALUES (@BlogTitle, @BlogAuthor, @BlogContent)";
            var parameters = new // anonymous object
            {
                blogRequest.BlogTitle,
                blogRequest.BlogAuthor,
                blogRequest.BlogContent
            };

            using IDbConnection db = new SqlConnection(DbConfig.DbConnection);
            var result = await db.ExecuteAsync(query, parameters);

            return result > 0 ? Ok("Saving Successful") : BadRequest("Saving Fail");
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteBlogAsync(int id)
    {
        try
        {
            string query = "DELETE FROM Tbl_Blog WHERE BlogId = @BlogId";
            using IDbConnection dbCon = new SqlConnection(DbConfig.DbConnection);
            var result = await dbCon.ExecuteAsync(query, new { BlogId = id });

            return result > 0 ? Ok("Deleting Successful!") : BadRequest("Deleting Fail!");
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogItemsAsync([FromBody] BlogRequestModel reqModel, int id)
    {
        try
        {
            string query = "UPDATE Tbl_Blog SET BlogTitle = @BlogTitle, BlogAuthor = @BlogAuthor WHERE BlogId = @BlogId";
            var parameters = new
            {
                BlogId = id,
                reqModel.BlogTitle,
                reqModel.BlogAuthor,
                reqModel.BlogContent
            };
            using IDbConnection db = new SqlConnection(DbConfig.DbConnection);
            var result = await db.ExecuteAsync(query, parameters);

            return result > 0 ? Ok("Updated Successfully!") : BadRequest("Updating Failed!");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchBlogAsync([FromBody] BlogRequestModel requestModel, int id)
    {
        try
        {
            if (id <= 0)
            { return BadRequest("Id is invalid!"); }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("BlogId", id);
            string conditions = string.Empty;

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                dynamicParameters.Add("@BlogTitle", requestModel.BlogTitle);
                conditions += "BlogTitle = @BlogTitle, ";

            }

            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                dynamicParameters.Add("@BlogAuthor", requestModel.BlogAuthor);
                conditions += "BlogAuthor = @BlogAuthor, ";

            }

            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                dynamicParameters.Add("@BlogContent", requestModel.BlogContent);
                conditions += "BlogContent = @BlogContent, ";
            }

            conditions = conditions.Substring(0, conditions.Length - 2);
            string query = @$"UPDATE Tbl_Blog SET {conditions} WHERE BlogId = @BlogId";

            using IDbConnection dbConnection = new SqlConnection(DbConfig.DbConnection);
            int result = await dbConnection.ExecuteAsync(query, dynamicParameters);

            return result > 0 ? Ok("Updating Successful!") : BadRequest("Updating Fail!");
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }
}
