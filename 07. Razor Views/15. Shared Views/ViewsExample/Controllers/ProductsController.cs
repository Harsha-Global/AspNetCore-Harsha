using Microsoft.AspNetCore.Mvc;

namespace ViewsExample.Controllers
{
  public class ProductsController : Controller
  {
    [Route("products/all")]
    public IActionResult All()
    {
      return View();
      //Views/Products/All.cshtml
      //Views/Shared/All.cshtml
    }
  }
}
