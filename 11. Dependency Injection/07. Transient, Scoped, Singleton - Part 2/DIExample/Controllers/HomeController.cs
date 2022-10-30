using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
  public class HomeController : Controller
  {
    private readonly ICitiesService _citiesService1;
    private readonly ICitiesService _citiesService2;
    private readonly ICitiesService _citiesService3;

    //constructor
    public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3)
    {
      _citiesService1 = citiesService1;
      _citiesService2 = citiesService2;
      _citiesService3 = citiesService3;
    }


    [Route("/")]
    public IActionResult Index()
    {
      List<string> cities = _citiesService1.GetCities();
      ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

      return View(cities);
    }
  }
}
