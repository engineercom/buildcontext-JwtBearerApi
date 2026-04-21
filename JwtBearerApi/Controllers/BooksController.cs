using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            
            var books = new string[] {
            "Üç Silahşörler",
            "Denemeler",
            "Sefiller",
            "Monte Kristo Kontu"

            };
            return Ok(books);
            
           
        }
    }
}
