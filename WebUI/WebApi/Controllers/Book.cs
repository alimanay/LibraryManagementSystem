using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("BookController is working!");
        }
    }

}
