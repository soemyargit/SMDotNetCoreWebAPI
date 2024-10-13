using Dapper;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SMDotNetCoreWebAPI.Models;
using SMDotNetCoreWebAPI.Queries;
using SMDotNetCoreWebAPI.Services;


namespace SMDotNetCoreWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BlogV1Controller : ControllerBase
    {
        
        
        private readonly DapperService _dapperService;

        public BlogV1Controller(DapperService dapperService)
        {
            _dapperService = dapperService;
        }

        #region HttpGet

        [HttpGet]

        public async Task<IActionResult> GetBlogQueryAsync()
        {
            try
            {
                string query = BlogQuery.GetBlogListQuery;
                var lst = await _dapperService.QueryAsync<BlogModel>(query);
                return Ok(lst);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #endregion

        #region HttpGetById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogQueryAsync(int id)
        {
            try
            {
                string query = BlogQuery.GetBlogByIDQuery;
                var lst = await _dapperService.QueryFirstOrDefaultAsync<BlogModel>(query,new {BlogId = id});
                return Ok(lst);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #endregion

        #region

        [HttpPost]

        public async Task<IActionResult> CreateBlogAsync([FromBody]BlogRequestModel reqModel)
        { 
            string query = BlogQuery.CreateBlogQuery;
            var parameters = new { reqModel.BlogTitle, reqModel.BlogAuthor, reqModel.BlogContent };
            int intResult = await _dapperService.ExecuteQueryAsync(query,parameters);
            return Ok(intResult);
        }

        #endregion

        #region HttpPut

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogModel([FromBody] BlogRequestModel reqModel,int id)
        {
            if (id <= 0) { return BadRequest( "Id is invalid." ); }           

            if (string.IsNullOrEmpty(reqModel.BlogTitle))
            { return BadRequest("Blog Title can not be empty."); }

            if (string.IsNullOrEmpty(reqModel.BlogAuthor))
            { return BadRequest("Blog Author can not be empty."); }

            if (string.IsNullOrEmpty(reqModel.BlogContent))
            { return BadRequest("Blog Content can not be empty."); }

            var query = BlogQuery.UpdateBlogQuery;

            var parameters = new {BlogId = id, reqModel.BlogTitle, reqModel.BlogAuthor, reqModel.BlogContent};
            int intResult = await _dapperService.ExecuteQueryAsync(query,parameters);
            return Ok(intResult);
        }
        #endregion

        #region

        [HttpPatch("{id}")]

        public async Task<IActionResult> PatchUpdateBlogAsync( [FromBody] BlogRequestModel reqModel,int id)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("BlogId", id);
                string conditionsSqlQuery = string.Empty;

                if (id <= 0) { return BadRequest("Invalid Id!"); }

                if (!string.IsNullOrEmpty(reqModel.BlogTitle)) { dynamicParameters.Add("@BlogTitle", reqModel.BlogTitle); conditionsSqlQuery += "BlogTitle = @BlogTitle, "; }
                if (!string.IsNullOrEmpty(reqModel.BlogAuthor)) { dynamicParameters.Add("@BlogAuthor", reqModel.BlogAuthor); conditionsSqlQuery += "BlogAuthor = @BlogAuthor, "; }
                if (!string.IsNullOrEmpty(reqModel.BlogContent)) { dynamicParameters.Add("@BlogContent", reqModel.BlogContent); conditionsSqlQuery += "BlogContent = @BlogContent, "; }
                conditionsSqlQuery = conditionsSqlQuery.Substring(0, conditionsSqlQuery.Length - 2);

                string query = @$"UPDATE Tbl_Blog SET {conditionsSqlQuery} WHERE BlogId = @BlogId";
                int intResult = await _dapperService.ExecuteQueryAsync(query, dynamicParameters);
                return Ok(intResult);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        #endregion

        #region [HttpDelete]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteBlogAsync(int id)
        {
            try
            {
                string queryString = BlogQuery.DeleteBlogQuery;
                var result = await _dapperService.ExecuteQueryAsync(queryString, new { BlogId = id });
                return result > 0 ? Ok("Deleting Successful.") : BadRequest("Deleting Fail!");

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #endregion


    }
}
