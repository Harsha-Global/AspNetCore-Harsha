using OrderManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a request to update an order.
 /// </summary>
 public class OrderUpdateRequest
 {
  /// <summary>
  /// Gets or sets the ID of the order.
  /// </summary>
  public Guid OrderId { get; set; }


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
  /// Maps the data from the current instance of <see cref="OrderUpdateRequest"/> to an instance of <see cref="Order"/>.
  /// </summary>
  /// <returns>An instance of <see cref="Order"/> with the mapped data.</returns>
  public Order ToOrder()
  {
   return new Order
   {
    OrderId = OrderId,
    CustomerName = CustomerName,
    OrderNumber = OrderNumber,
    OrderDate = OrderDate,
    TotalAmount = TotalAmount,
   };
  }
 }
}
