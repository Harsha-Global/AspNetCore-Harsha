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
      //ViewBag.ClientID = _configuration["weatherapi:ClientID"];
      //ViewBag.ClientSecret = _configuration.GetValue("weatherapi:ClientSecret", "the default client secret");

      IConfigurationSection wetherapiSection = _configuration.GetSection("weatherapi");

      ViewBag.ClientID = wetherapiSection["ClientID"];

      ViewBag.ClientSecret = wetherapiSection["ClientSecret"];

      return View();
    }
  }
}
