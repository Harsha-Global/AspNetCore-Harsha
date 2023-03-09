using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
 [Area("Admin")]
 public class HomeController : Controller
 {
  public IActionResult Index()
  {
   return View();
  }
 }
}
