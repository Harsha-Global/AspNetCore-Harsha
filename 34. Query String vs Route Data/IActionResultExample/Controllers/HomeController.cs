using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("bookstore/{bookid?}/{isloggedin?}")]
    //Url: /bookstore?bookid=10&isloggedin=true
    public IActionResult Index(int? bookid, bool? isloggedin)
    {
      //Book id should be applied
      if (bookid.HasValue == false)
      {
        //return new BadRequestResult();
        return BadRequest("Book id is not supplied or empty");
      }

      //Book id can't be less than or equal to 0
      if (bookid <= 0)
      {
        return BadRequest("Book id can't be less than or equal to 0");
      }

      //Book id should be between 1 to 1000
      if (bookid <= 0)
      {
        return BadRequest("Book id can't be less than or equal to zero");
      }
      if (bookid > 1000)
      {
        return NotFound("Book id can't be greater than 1000");
      }

      //isloggedin should be true
      if (isloggedin == false)
      {
        return StatusCode(401);
      }

      return Content($"Book id: {bookid}", "text/plain");
    }
  }
}
