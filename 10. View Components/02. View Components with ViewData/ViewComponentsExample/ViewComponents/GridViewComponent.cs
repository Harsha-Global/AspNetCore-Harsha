using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
  public class GridViewComponent : ViewComponent
  {
    public async Task<IViewComponentResult> InvokeAsync()
    {
      PersonGridModel model = new PersonGridModel()
      {
        GridTitle = "Persons List",
        Persons = new List<Person>() {
          new Person() { PersonName = "John", JobTitle = "Manager" },
          new Person() { PersonName = "Jones", JobTitle = "Asst. Manager" },
          new Person() { PersonName = "William", JobTitle = "Clerk" },
        }
      };
      ViewData["Grid"] = model;
      return View("Sample"); //invoked a partial view Views/Shared/Components/Grid/Default.cshtml
    }
  }
}
