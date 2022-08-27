using ServiceContracts;

namespace Services
{
  public class CitiesService : ICitiesService
  {
    private List<string> _cities;

    private Guid _servieInstanceId;

    public Guid ServiceInstanceId
    {
      get
      {
        return _servieInstanceId;
      }
    }

    public CitiesService()
    {
      _servieInstanceId = Guid.NewGuid();
      _cities = new List<string>() { 
        "London",
        "Paris",
        "New York",
        "Tokyo",
        "Rome"
      };
    }

    public List<string> GetCities()
    {
      return _cities;
    }
  }
}
