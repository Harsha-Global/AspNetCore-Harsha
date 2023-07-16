using OrderManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a request to add an order.
 /// </summary>
 public class OrderAddRequest
 {
  /// <summary>
  /// Gets or sets the name of the customer associated with the order.
  /// </summary>
  [Required(ErrorMessage = "The CustomerName field is required.")]
  [StringLength(50, ErrorMessage = "The CustomerName field must not exceed 50 characters.")]
  public string? CustomerName { get; set; }


  /// <summary>
  /// Gets or sets the order number.
  /// </summary>
  [Required(ErrorMessage = "The OrderNumber field is required.")]
  [RegularExpression(@"^(?i)ORD_\d{4}_\d+$\r\n", ErrorMessage = "The Order number should begin with 'ORD' followed by an underscore (_) and a sequential number.")]
  public string? OrderNumber { get; set; }


  /// <summary>
  /// Gets or sets the date of the order.
  /// </summary>
  [Required(ErrorMessage = "The OrderDate field is required.")]
  public DateTime OrderDate { get; set; }


  /// <summary>
  /// Gets or sets the total amount of the order.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The TotalAmount field must be a positive number.")]
  public decimal TotalAmount { get; set; }


  /// <summary>
  /// Gets or sets the list of order items associated with the order.
  /// </summary>
  public List<OrderItemAddRequest> OrderItems { get; set; } = new List<OrderItemAddRequest>();


  /// <summary>
  /// Converts the <see cref="OrderAddRequest"/> object to an <see cref="Order"/> object.
  /// </summary>
  /// <returns>The converted <see cref="Order"/> object.</returns>
  public Order ToOrder()
  {
   return new Order
   {
    CustomerName = CustomerName,
    OrderNumber = OrderNumber,
    OrderDate = OrderDate,
    TotalAmount = TotalAmount
   };
  }
 }
}
