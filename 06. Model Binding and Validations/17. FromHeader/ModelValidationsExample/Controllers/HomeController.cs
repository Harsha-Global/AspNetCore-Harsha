using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.Models;
using ModelValidationsExample.CustomModelBinders;

namespace ModelValidationsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("register")]
    public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")] string UserAgent)
    {
      if (!ModelState.IsValid)
      {
        //get error messages from model state
        string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
  
        return BadRequest(errors);
      }

      return Content($"{person}, {UserAgent}");
    }
  }
}
