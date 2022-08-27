using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("home")]
    [Route("/")]
    public IActionResult Index()
    {
      ViewData["appTitle"] = "Asp.Net Core Demo App";

      List<Person> people = new List<Person>()
      {
          new Person() { Name = "John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
          new Person() { Name = "Linda", DateOfBirth = DateTime.Parse("2005-01-09"), PersonGender = Gender.Female},
          new Person() { Name = "Susan", DateOfBirth = null, PersonGender = Gender.Other}
      };
      //ViewData["people"] = people;
      ViewBag.people = people;

      return View(); //Views/Home/Index.cshtml
    }
  }
}
