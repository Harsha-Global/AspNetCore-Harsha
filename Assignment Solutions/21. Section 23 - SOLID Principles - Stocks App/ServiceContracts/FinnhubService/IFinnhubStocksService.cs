using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.FinnhubService
{
 /// <summary>
 /// Represents a service that makes HTTP requests to finnhub.io
 /// </summary>
 public interface IFinnhubStocksService
 {
  /// <summary>
  /// Returns list of all stocks supported by an exchange (default: US)
  /// </summary>
  /// <returns>List of stocks</returns>
  Task<List<Dictionary<string, string>>?> GetStocks();
 }
}

