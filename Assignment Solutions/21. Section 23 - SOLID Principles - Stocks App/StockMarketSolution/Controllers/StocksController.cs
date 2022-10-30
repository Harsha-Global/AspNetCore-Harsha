using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.FinnhubService;
using StockMarketSolution.Models;
using System.Diagnostics;
using System.Text.Json;

namespace StockMarketSolution.Controllers
{
 [Route("[controller]")]
 public class StocksController : Controller
 {
  private readonly TradingOptions _tradingOptions;
  private readonly IFinnhubStocksService _finnhubStocksService;


  /// <summary>
  /// Constructor for TradeController that executes when a new object is created for the class
  /// </summary>
  /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
  /// <param name="finnhubStocksService">Injecting FinnhubStocksService</param>
  public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubStocksService finnhubStocksService)
  {
   _tradingOptions = tradingOptions.Value;
   _finnhubStocksService = finnhubStocksService;
  }


  [Route("/")]
  [Route("[action]/{stock?}")]
  [Route("~/[action]/{stock?}")]
  public async Task<IActionResult> Explore(string? stock, bool showAll = false)
  {
   //get company profile from API server
   List<Dictionary<string, string>>? stocksDictionary = await _finnhubStocksService.GetStocks();

   List<Stock> stocks = new List<Stock>();

   if (stocksDictionary is not null)
   {
    //filter stocks
    if (!showAll && _tradingOptions.Top25PopularStocks != null)
    {
     string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
     if (Top25PopularStocksList is not null)
     {
      stocksDictionary = stocksDictionary
       .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
       .ToList();
     }
    }

    //convert dictionary objects into Stock objects
    stocks = stocksDictionary
     .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
    .ToList();
   }

   ViewBag.stock = stock;
   return View(stocks);
  }
 }
}


