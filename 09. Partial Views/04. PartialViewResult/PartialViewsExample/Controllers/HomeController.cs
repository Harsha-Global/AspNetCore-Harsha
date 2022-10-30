using Microsoft.AspNetCore.Mvc;
using PartialViewsExample.Models;

namespace PartialViewsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("/")]
    public IActionResult Index()
    {
      return View();
    }

    [Route("about")]
    public IActionResult About()
    {
      return View();
    }

    [Route("programming-languages")]
    public IActionResult ProgrammingLanguages()
    {
      ListModel listModel = new ListModel() {
        ListTitle = "Programming Languages List",
        ListItems = new List<string>() { 
          "Python",
          "C#",
          "Go"
        }
      };

      return PartialView("_ListPartialView", listModel);
    }
  }
}
