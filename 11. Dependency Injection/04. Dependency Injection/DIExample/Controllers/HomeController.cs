using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
  public class HomeController : Controller
  {
    private readonly ICitiesService _citiesService;

    //constructor
    public HomeController(ICitiesService citiesService)
    {
      //create object of CitiesService class
      _citiesService = citiesService; //new CitiesService();
    }

    [Route("/")]
    public IActionResult Index()
    {
      List<string> cities = _citiesService.GetCities();
      return View(cities);
    }
  }
}
