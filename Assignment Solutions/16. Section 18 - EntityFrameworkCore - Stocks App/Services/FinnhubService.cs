using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
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
 }
}

/*
User Secrets:
dotnet user-secrets init --project StockMarketSolution
dotnet user-secrets set "FinnhubToken" "cc676uaad3i9rj8tb1s0" --project StockMarketSolution
*/
