namespace WeatherSolution.Models
{
 public class CityWeather
 {
  public string CityUniqueCode { get; set; } = "";
  public string CityName { get; set; } = "";
  public DateTime DateAndTime { get; set; }
  public int TemperatureFahrenheit { get; set; }
}
}

