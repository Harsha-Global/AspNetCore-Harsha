using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExample.Controllers
{
  public class HomeController : Controller
  {
    //private field
    private readonly IConfiguration _configuration;

    //constructor
    public HomeController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [Route("/")]
    public IActionResult Index()
    {
      //Bind: Loads configuration values into a new Options object
      //WeatherApiOptions options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();

      //Bind: Loads configuration values into existing Options object
      WeatherApiOptions options = new WeatherApiOptions();
      _configuration.GetSection("weatherapi").Bind(options);

      ViewBag.ClientID = options.ClientID;
      ViewBag.ClientSecret = options.ClientSecret;

      return View();
    }
  }
}
