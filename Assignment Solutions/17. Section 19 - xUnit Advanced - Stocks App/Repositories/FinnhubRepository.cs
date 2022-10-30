using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Net.Http;
using System.Text.Json;


namespace Repositories
{
 public class FinnhubRepository : IFinnhubRepository
 {
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IConfiguration _configuration;


  public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
   _httpClientFactory = httpClientFactory;
   _configuration = configuration;
  }


  public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
  {
   //create http client
   HttpClient httpClient = _httpClientFactory.CreateClient();

   //create http request
   HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
   {
    Method = HttpMethod.Get,
    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
   };

   //send request
   HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

   //read response body
   string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

   //convert response body (from JSON into Dictionary)
   Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

   if (responseDictionary == null)
    throw new InvalidOperationException("No response from server");

   if (responseDictionary.ContainsKey("error"))
    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

   //return response dictionary back to the caller
   return responseDictionary;
  }


  public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
  {
   //create http client
   HttpClient httpClient = _httpClientFactory.CreateClient();

   //create http request
   HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
   {
    Method = HttpMethod.Get,
    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
   };

   //send request
   HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

   //read response body
   string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

   //convert response body (from JSON into Dictionary)
   Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

   if (responseDictionary == null)
    throw new InvalidOperationException("No response from server");

   if (responseDictionary.ContainsKey("error"))
    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

   //return response dictionary back to the caller
   return responseDictionary;
  }


  public async Task<List<Dictionary<string, string>>?> GetStocks()
  {
   //create http client
   HttpClient httpClient = _httpClientFactory.CreateClient();

   //create http request
   HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
   {
    Method = HttpMethod.Get,
    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}") //URI includes the secret token
   };

   //send request
   HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

   //read response body
   string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

   //convert response body (from JSON into Dictionary)
   List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

   if (responseDictionary == null)
    throw new InvalidOperationException("No response from server");

   //return response dictionary back to the caller
   return responseDictionary;
  }


  public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
  {
   //create http client
   HttpClient httpClient = _httpClientFactory.CreateClient();

   //create http request
   HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
   {
    Method = HttpMethod.Get,
    RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
   };

   //send request
   HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

   //read response body
   string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

   //convert response body (from JSON into Dictionary)
   Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

   if (responseDictionary == null)
    throw new InvalidOperationException("No response from server");

   if (responseDictionary.ContainsKey("error"))
    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

   //return response dictionary back to the caller
   return responseDictionary;
  }
 }
}

/*
User Secrets:
dotnet user-secrets init --project StockMarketSolution
dotnet user-secrets set "FinnhubToken" "cc676uaad3i9rj8tb1s0" --project StockMarketSolution
*/
