using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a buy order to purchase the stocks
 /// </summary>
 public class BuyOrder
 {
  /// <summary>
  /// The unique ID of the buy order
  /// </summary>
  public Guid BuyOrderID { get; set; }


  /// <summary>
  /// The unique symbol of the stock
  /// </summary>
  public string StockSymbol { get; set; }


  /// <summary>
  /// The company name of the stock
  /// </summary>
  [Required(ErrorMessage = "Stock Name can't be null or empty")]
  public string StockName { get; set; }


  /// <summary>
  /// Date and time of order, when it is placed by the user
  /// </summary>
  public DateTime DateAndTimeOfOrder { get; set; }


  /// <summary>
  /// The number of stocks (shares) to buy
  /// </summary>
  [Range(1, 100000, ErrorMessage = "You can buy maximum of 100000 shares in single order. Minimum is 1.")]
  public uint Quantity { get; set; }


  /// <summary>
  /// The price of each stock (share)
  /// </summary>
  [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
  public double Price { get; set; }
 }
}

