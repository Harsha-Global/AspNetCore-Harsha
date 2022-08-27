using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("bookstore")]
    //Url: /bookstore?bookid=10&isloggedin=true
    public IActionResult Index()
    {
      //Book id should be applied
      if (!Request.Query.ContainsKey("bookid"))
      {
        //return new BadRequestResult();
        return BadRequest("Book id is not supplied");
      }

      //Book id can't be empty
      if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
      {
        return BadRequest("Book id can't be null or empty");
      }

      //Book id should be between 1 to 1000
      int bookId = Convert.ToInt16(ControllerContext.HttpContext.Request.Query["bookid"]);
      if (bookId <= 0)
      {
        return BadRequest("Book id can't be less than or equal to zero");
      }
      if (bookId > 1000)
      {
        return NotFound("Book id can't be greater than 1000");
      }

      //isloggedin should be true
      if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
      {
        //return Unauthorized("User must be authenticated");
        return StatusCode(401);
      }

      //302 - Found - RedirectToActionResult
      //return new RedirectToActionResult("Books", "Store", new { id = bookId }); //302 - Found
      //return RedirectToAction("Books", "Store", new { id = bookId });

      //301 - Moved Permanently - RedirectToActionResult
      //return new RedirectToActionResult("Books", "Store", new { }, permanent: true); //301 - Moved Permanently
      //return RedirectToActionPermanent("Books", "Store", new { id = bookId });

      //302 - Found - LocalRedirectResult
      //return new LocalRedirectResult($"store/books/{bookId}"); //302 - Found
      //return LocalRedirect($"store/books/{bookId}"); //302 - Found

      //301 - Moved Permanently - LocalRedirectResult
      return new LocalRedirectResult($"store/books/{bookId}", true); //301 - Moved Permanently
      //return LocalRedirectPermanent($"store/books/{bookId}"); //301 - Moved Permanently

      //return Redirect($"store/books/{bookId}"); //302 - Found
      //return RedirectPermanent($"store/books/{bookId}"); //301 - Moved Permanently
    }
  }
}
