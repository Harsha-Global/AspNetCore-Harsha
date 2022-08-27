using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("register")]
    public IActionResult Index(Person person)
    {
      if (!ModelState.IsValid)
      {
        string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
  
        return BadRequest(errors);
      }

      return Content($"{person}");
    }
  }
}
