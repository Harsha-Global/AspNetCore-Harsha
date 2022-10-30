using WeatherSolution.Models;

namespace ServiceContracts
{
 /// <summary>
 /// Represents a service contract that handles weather details of cities
 /// </summary>
 public interface IWeatherService
 {
  /// <summary>
  /// Returns a list of CityWeather objects that contains weather details of cities
  /// </summary>
  /// <returns>List of CityWeather objects that contains weather details of cities</returns>
  List<CityWeather> GetWeatherDetails();

  /// <summary>
  /// Returns an object of CityWeather based on the given city code
  /// </summary>
  /// <param name="CityCode">CityCode to search</param>
  /// <returns>CityWeather object that contains weather details of the selected city</returns>
  CityWeather? GetWeatherByCityCode(string CityCode);
 }
}

