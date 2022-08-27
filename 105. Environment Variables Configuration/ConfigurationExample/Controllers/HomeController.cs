using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
  public class HomeController : Controller
  {
    //private field
    private readonly WeatherApiOptions _options;

    //constructor
    public HomeController(IOptions<WeatherApiOptions> weatherApiOptions)
    {
      _options = weatherApiOptions.Value;
    }

    [Route("/")]
    public IActionResult Index()
    {
      ViewBag.ClientID = _options.ClientID;
      ViewBag.ClientSecret = _options.ClientSecret;

      return View();
    }
  }
}
