using Microsoft.AspNetCore.Mvc;

namespace ViewsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("home")]
    public IActionResult Index()
    {
      return View(); //Views/Home/Index.cshtml
      //return View("abc"); //abc.cshtml
      //return new ViewResult() { ViewName = "abc" };
    }
  }
}
