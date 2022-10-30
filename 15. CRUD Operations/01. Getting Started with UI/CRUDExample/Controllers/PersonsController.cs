using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
  public class PersonsController : Controller
  {
    [Route("persons/index")]
    [Route("/")]
    public IActionResult Index()
    {
      return View();
    }
  }
}
