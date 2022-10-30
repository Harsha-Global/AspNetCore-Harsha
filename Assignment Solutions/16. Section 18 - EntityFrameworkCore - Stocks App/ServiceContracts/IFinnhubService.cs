using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
 /// <summary>
 /// Represents a service that makes HTTP requests to finnhub.io
 /// </summary>
 public interface IFinnhubService
 {
  /// <summary>
  /// Returns company details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.
  /// </summary>
  /// <param name="stockSymbol">Stock symbol to search</param>
  /// <returns>Returns a dictionary that contains details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.</returns>
  Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);


  /// <summary>
  /// Returns stock price details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price
  /// </summary>
  /// <param name="stockSymbol">Stock symbol to search</param>
  /// <returns>Returns a dictionary that contains details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price</returns>
  Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
 }
}

