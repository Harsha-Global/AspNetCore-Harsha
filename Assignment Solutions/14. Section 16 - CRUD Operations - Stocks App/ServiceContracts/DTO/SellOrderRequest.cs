using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// DTO class that represents a sell order - that can be used while inserting / updating
 /// </summary>
 public class SellOrderRequest : IValidatableObject
 {
  /// <summary>
  /// The unique symbol of the stock
  /// </summary>
  [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
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
  /// The number of stocks (shares) to sell
  /// </summary>
  [Range(1, 100000, ErrorMessage = "You can sell maximum of 100000 shares in single order. Minimum is 1.")]
  public uint Quantity { get; set; }


  /// <summary>
  /// The price of each stock (share)
  /// </summary>
  [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
  public double Price { get; set; }


  /// <summary>
  /// Converts the current object of SellOrderRequest into a new object of SellOrder type
  /// </summary>
  /// <returns>A new object of SellOrder class</returns>
  public SellOrder ToSellOrder()
  {
   //create new object of SellOrder class
   return new SellOrder() { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
  }


  /// <summary>
  /// Model class-level validation using IValidatableObject
  /// </summary>
  /// <param name="validationContext">ValidationContext to validate</param>
  /// <returns>Returns validation errors as ValidationResult</returns>
  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
   List<ValidationResult> results = new List<ValidationResult>();

   //Date of order should be less than Jan 01, 2000
   if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
   {
    results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
   }

   return results;
  }
 }
}

