using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
 public class HomeController : Controller
 {
  [Route("Error")]
  public IActionResult Error()
  {
   IExceptionHandlerPathFeature ? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
   if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
   {
    ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
   }
   return View(); //Views/Shared/Error
  }
 }
}
