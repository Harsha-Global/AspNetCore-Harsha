using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Controllers
{
  public class PersonsController : Controller
  {
    //private fields
    private readonly IPersonsService _personsService;

    //constructor
    public PersonsController(IPersonsService personsService)
    {
      _personsService = personsService;
    }

    [Route("persons/index")]
    [Route("/")]
    public IActionResult Index()
    {
      List<PersonResponse> persons = _personsService.GetAllPersons();

      return View(persons); //Views/Persons/Index.cshtml
    }
  }
}
