using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.CustomModelBinders;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
 public class HomeController : Controller
 {
  [Route("register")]
  public IActionResult Index([ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
  {
   if (!ModelState.IsValid)
   {
    //get error messages from model state
    string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

    return BadRequest(errors);
   }

   return Content($"{person}");
  }
 }
}
