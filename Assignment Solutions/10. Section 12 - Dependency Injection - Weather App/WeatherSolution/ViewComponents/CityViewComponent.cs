using WeatherSolution.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherSolution.ViewComponents
{
 public class CityViewComponent : ViewComponent
 {
  public async Task<IViewComponentResult> InvokeAsync(CityWeather city)
  {
   ViewBag.CityCssClass = GetCssClassByFahrenheit(city.TemperatureFahrenheit);

   return View(city); //invokes view of the view component at Views/Shared/Components/Grid/Sample.cshtml
  }

  //private method: get css class based on ranges of fahrenheit value as instructed in the requirement
  private string GetCssClassByFahrenheit(int TemperatureFahrenheit)
  {
   return TemperatureFahrenheit switch
   {
    (< 44) => "blue-back",
    (>= 44) and (< 75) => "green-back",
    (>= 75) => "orange-back"
   };
  }
 }
}

