using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
 [Route("[controller]")]
 public class TradeController : Controller
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
  public TradeController(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
  {
   _tradingOptions = tradingOptions.Value;
   _stocksService = stocksService;
   _finnhubService = finnhubService;
   _configuration = configuration;
  }


  [Route("/")]
  [Route("[action]")]
  [Route("~/[controller]")]
  public IActionResult Index()
  {
   //reset stock symbol if not exists
   if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
    _tradingOptions.DefaultStockSymbol = "MSFT";


   //get company profile from API server
   Dictionary<string, object>? companyProfileDictionary = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

   //get stock price quotes from API server
   Dictionary<string, object>? stockQuoteDictionary = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);


   //create model object
   StockTrade stockTrade = new StockTrade() { StockSymbol = _tradingOptions.DefaultStockSymbol };

   //load data from finnHubService into model object
   if (companyProfileDictionary != null && stockQuoteDictionary != null)
   {
    stockTrade = new StockTrade() { StockSymbol = companyProfileDictionary["ticker"].ToString(), StockName = companyProfileDictionary["name"].ToString(), Quantity = _tradingOptions.DefaultOrderQuantity ?? 0, Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
   }

   //Send Finnhub token to view
   ViewBag.FinnhubToken = _configuration["FinnhubToken"];

   return View(stockTrade);
  }


  [Route("[action]")]
  [HttpPost]
  public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
  {
   //update date of order
   buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

   //re-validate the model object after updating the date
   ModelState.Clear();
   TryValidateModel(buyOrderRequest);


   if (!ModelState.IsValid)
   {
    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
    return View("Index", stockTrade);
   }

   //invoke service method
   BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);

   return RedirectToAction(nameof(Orders));
  }


  [Route("[action]")]
  [HttpPost]
  public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
  {
   //update date of order
   sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

   //re-validate the model object after updating the date
   ModelState.Clear();
   TryValidateModel(sellOrderRequest);

   if (!ModelState.IsValid)
   {
    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
    return View("Index", stockTrade);
   }

   //invoke service method
   SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);

   return RedirectToAction(nameof(Orders));
  }


  [Route("[action]")]
  public IActionResult Orders()
  {
   //invoke service methods
   List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();
   List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

   //create model object
   Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

   ViewBag.TradingOptions = _tradingOptions;

   return View(orders);
  }
 }
}


