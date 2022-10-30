using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StockMarketSolution.ViewComponents
{
 public class SelectedStockViewComponent : ViewComponent
 {
  private readonly TradingOptions _tradingOptions;
  private readonly IStocksService _stocksService;
  private readonly IFinnhubService _finnhubService;
  private readonly IConfiguration _configuration;


  /// <summary>
  /// Constructor for TradeController that executes when a new object is created for the class
  /// </summary>
  /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
  /// <param name="stocksService">Injecting StocksService</param>
  /// <param name="finnhubService">Injecting FinnhubService</param>
  /// <param name="configuration">Injecting IConfiguration</param>
  public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
  {
   _tradingOptions = tradingOptions.Value;
   _stocksService = stocksService;
   _finnhubService = finnhubService;
   _configuration = configuration;
  }

  public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
  {
   Dictionary<string, object>? companyProfileDict = null;

   if (stockSymbol != null)
   {
    companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
    var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
    if (stockPriceDict != null && companyProfileDict != null)
    {
     companyProfileDict.Add("price", stockPriceDict["c"]);
    }
   }

   if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
    return View(companyProfileDict);
   else
    return Content("");
  }
 }
}

