using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExceptionFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
 [Route("[controller]")]
 //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3)]

 [ResponseHeaderFilterFactory("My-Key-From-Controller", "My-Value-From-Controller", 3)]
 [TypeFilter(typeof(HandleExceptionFilter))]
 [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
 public class PersonsController : Controller
 {
  //private fields
  private readonly IPersonsService _personsService;
  private readonly ICountriesService _countriesService;
  private readonly ILogger<PersonsController> _logger;

  //constructor
  public PersonsController(IPersonsService personsService, ICountriesService countriesService, ILogger<PersonsController> logger)
  {
   _personsService = personsService;
   _countriesService = countriesService;
   _logger = logger;
  }

  //Url: persons/index
  [Route("[action]")]
  [Route("/")]
  [ServiceFilter(typeof(PersonsListActionFilter), Order = 4)]

  //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "MyKey-FromAction", "MyValue-From-Action", 1 }, Order = 1)]

  [ResponseHeaderFilterFactory("MyKey-FromAction", "MyValue-From-Action", 1)]

  [TypeFilter(typeof(PersonsListResultFilter))]
  [SkipFilter]
  public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
  {
   _logger.LogInformation("Index action method of PersonsController");

   _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");


   //Search
   List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);

   //Sort
   List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);

   return View(sortedPersons); //Views/Persons/Index.cshtml
  }


  //Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
  //Url: persons/create
  [Route("[action]")]
  [HttpGet]
  [ResponseHeaderFilterFactory("my-key", "my-value", 4)]
  public async Task<IActionResult> Create()
  {
   List<CountryResponse> countries = await _countriesService.GetAllCountries();
   ViewBag.Countries = countries.Select(temp =>
     new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() }
   );

   //new SelectListItem() { Text="Harsha", Value="1" }
   //<option value="1">Harsha</option>
   return View();
  }


  [HttpPost]
  //Url: persons/create
  [Route("[action]")]
  [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
  [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] {  false })]
  public async Task<IActionResult> Create(PersonAddRequest personRequest)
  {
   //call the service method
   PersonResponse personResponse = await _personsService.AddPerson(personRequest);

   //navigate to Index() action method (it makes another get request to "persons/index"
   return RedirectToAction("Index", "Persons");
  }


  [HttpGet]
  [Route("[action]/{personID}")] //Eg: /persons/edit/1
  //[TypeFilter(typeof(TokenResultFilter))]
  public async Task<IActionResult> Edit(Guid personID)
  {
   PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
   if (personResponse == null)
   {
    return RedirectToAction("Index");
   }

   PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

   List<CountryResponse> countries = await _countriesService.GetAllCountries();
   ViewBag.Countries = countries.Select(temp =>
   new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

   return View(personUpdateRequest);
  }


  [HttpPost]
  [Route("[action]/{personID}")]
  [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
  [TypeFilter(typeof(TokenAuthorizationFilter))]
  public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
  {
   PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personRequest.PersonID);

   if (personResponse == null)
   {
    return RedirectToAction("Index");
   }

   PersonResponse updatedPerson = await _personsService.UpdatePerson(personRequest);
   return RedirectToAction("Index");
  }


  [HttpGet]
  [Route("[action]/{personID}")]
  public async Task<IActionResult> Delete(Guid? personID)
  {
   PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
   if (personResponse == null)
    return RedirectToAction("Index");

   return View(personResponse);
  }

  [HttpPost]
  [Route("[action]/{personID}")]
  public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
  {
   PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateResult.PersonID);
   if (personResponse == null)
    return RedirectToAction("Index");

   await _personsService.DeletePerson(personUpdateResult.PersonID);
   return RedirectToAction("Index");
  }


  [Route("PersonsPDF")]
  public async Task<IActionResult> PersonsPDF()
  {
   //Get list of persons
   List<PersonResponse> persons = await _personsService.GetAllPersons();

   //Return view as pdf
   return new ViewAsPdf("PersonsPDF", persons, ViewData)
   {
    PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
   };
  }


  [Route("PersonsCSV")]
  public async Task<IActionResult> PersonsCSV()
  {
   MemoryStream memoryStream = await _personsService.GetPersonsCSV();
   return File(memoryStream, "application/octet-stream", "persons.csv");
  }


  [Route("PersonsExcel")]
  public async Task<IActionResult> PersonsExcel()
  {
   MemoryStream memoryStream = await _personsService.GetPersonsExcel();
   return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
  }
 }
}
