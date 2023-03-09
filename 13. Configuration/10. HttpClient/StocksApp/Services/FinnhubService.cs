using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services
{
 public class FinnhubService : IFinnhubService
 {
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IConfiguration _configuration;

  public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
   _httpClientFactory = httpClientFactory;
   _configuration = configuration;
  }

  public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
  {
   using (HttpClient httpClient = _httpClientFactory.CreateClient())
   {
    HttpRequestMessage httpRequestMessage = new HttpRequestMessage() { 
     RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
     Method = HttpMethod.Get
    };

    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

    Stream stream = httpResponseMessage.Content.ReadAsStream();

    StreamReader streamReader= new StreamReader(stream);

    string response = streamReader.ReadToEnd();
    Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

    if (responseDictionary == null)
     throw new InvalidOperationException("No response from finnhub server");

    if (responseDictionary.ContainsKey("error"))
     throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

    return responseDictionary;
   }
  }
 }
}
