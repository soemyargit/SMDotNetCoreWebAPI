namespace SMDotNetCoreADO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly ADOService _aDOService;
    private readonly AdoDotNetService _service;

    public BlogController(ADOService aDOService, AdoDotNetService service)
    {
        _aDOService = aDOService;
        _service = service;
    }

    #region HttpGet

    [HttpGet]
    public ActionResult GetBlogModel()
    {
        try
        {
            string sqlQuery = BlogQuery.GetBlogListQuery;
            var lst = _aDOService.cmdExecReader(sqlQuery);

            return Ok(lst);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    #endregion

    #region HttpGetById

    [HttpGet("{id}")]
    public ActionResult GetBlogModelById(int id)
    {
        try
        {
            string sqlQuery =
                @$"SELECT BlogId, BlogTitle, BlogAuthor, BlogContent FROM Tbl_Blog WHERE BlogId = {id}";
            List<BlogModel> lstBlogModel = new List<BlogModel>();
            lstBlogModel = _aDOService.cmdExecReader(sqlQuery);
            return Ok(lstBlogModel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    #endregion

    #region HttpPost
    [HttpPost]
    public ActionResult CreateBlog([FromBody] BlogRequestModel breqModel)
    {
        string sqlQuery = BlogQuery.CreateBlogQuery;
        int iResult = _aDOService.ExecuteNonQuery(sqlQuery, "", breqModel);

        return Ok(iResult);
    }

    #endregion

    #region HttpPut

    [HttpPut]
    public ActionResult UpdateBlog([FromBody] BlogRequestModel blogRequest, int id)
    {
        string sqlQuery = BlogQuery.UpdateBlogQuery;
        int iResult = _aDOService.ExecuteNonQuery(sqlQuery, "", blogRequest, id);

        return Ok(iResult);
    }

    #endregion

    #region HttpDelete

    [HttpDelete("{id}")]
    public ActionResult DeleteBlog(int id)
    {
        string sqlQuery = BlogQuery.DeleteBlogQuery;
        BlogRequestModel breqModel = new BlogRequestModel();
        int iResult = _aDOService.ExecuteNonQuery(sqlQuery, "D", breqModel, id);

        return Ok(iResult);
    }

    #endregion
}
