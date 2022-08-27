using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
 [Route("[controller]")]
 public class CountriesController : Controller
 {
  [Route("UploadFromExcel")]
  public IActionResult UploadFromExcel()
  {
   return View();
  }
 }
}
