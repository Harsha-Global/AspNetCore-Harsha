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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    //constructor
    public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, IServiceScopeFactory serviceScopeFactory)
    {
      _citiesService1 = citiesService1;
      _citiesService2 = citiesService2;
      _citiesService3 = citiesService3;
      _serviceScopeFactory = serviceScopeFactory;
    }


    [Route("/")]
    public IActionResult Index()
    {
      List<string> cities = _citiesService1.GetCities();
      ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

      using (IServiceScope scope = _serviceScopeFactory.CreateScope())
      {
        //Inject CitiesService
        ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
        //DB work

        ViewBag.InstanceId_CitiesServicece_InScope = citiesService.ServiceInstanceId;
      } //end of scope; it calls CitiesService.Dispose()

        return View(cities);
    }
  }
}
