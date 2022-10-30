using Autofac;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers
{
  public class HomeController : Controller
  {
    private readonly ICitiesService _citiesService1;
    private readonly ICitiesService _citiesService2;
    private readonly ICitiesService _citiesService3;
    private readonly ILifetimeScope _lifeTimeScope;

    //constructor
    public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, ILifetimeScope serviceScopeFactory)
    {
      _citiesService1 = citiesService1;
      _citiesService2 = citiesService2;
      _citiesService3 = citiesService3;
      _lifeTimeScope = serviceScopeFactory;
    }


    [Route("/")]
    public IActionResult Index()
    {
      List<string> cities = _citiesService1.GetCities();
      ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;

      ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

      using (ILifetimeScope scope = _lifeTimeScope.BeginLifetimeScope())
      {
        //Inject CitiesService
        ICitiesService citiesService = scope.Resolve<ICitiesService>();
        //DB work

        ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
      } //end of scope; it calls CitiesService.Dispose()

      return View(cities);
    }
  }
}
