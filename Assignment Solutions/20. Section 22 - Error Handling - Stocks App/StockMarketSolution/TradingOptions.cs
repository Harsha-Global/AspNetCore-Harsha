namespace StockMarketSolution
{
 /// <summary>
 /// Represents Options pattern for "StockPrice" configuration
 /// </summary>
 public class TradingOptions
 {
  public uint? DefaultOrderQuantity { get; set; }
  public string? Top25PopularStocks { get; set; }
 }
}

