using OrderManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a request to add an order item.
 /// </summary>
 public class OrderItemAddRequest
 {
  /// <summary>
  /// Gets or sets the ID of the order associated with the order item.
  /// </summary>
  public Guid OrderId { get; set; }


  /// <summary>
  /// Gets or sets the name of the product associated with the order item.
  /// </summary>
  [Required(ErrorMessage = "The ProductName field is required.")]
  [StringLength(50, ErrorMessage = "The ProductName field must not exceed 50 characters.")]
  public string? ProductName { get; set; }


  /// <summary>
  /// Gets or sets the quantity of the product in the order item.
  /// </summary>
  [Range(1, int.MaxValue, ErrorMessage = "The Quantity field must be a positive number.")]
  public int Quantity { get; set; }


  /// <summary>
  /// Gets or sets the unit price of the product in the order item.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The UnitPrice field must be a positive number.")]
  public decimal UnitPrice { get; set; }


  /// <summary>
  /// Gets or sets the total price of the order item.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The total price of the order item.")]
  public decimal TotalPrice { get; set; }


  /// <summary>
  /// Converts the <see cref="OrderItemAddRequest"/> object to an <see cref="OrderItem"/> object.
  /// </summary>
  /// <returns>The converted <see cref="OrderItem"/> object.</returns>
  public OrderItem ToOrderItem()
  {
   return new OrderItem
   {
    OrderId = OrderId,
    ProductName = ProductName,
    Quantity = Quantity,
    UnitPrice = UnitPrice,
    TotalPrice = TotalPrice
   };
  }
 }


 /// <summary>
 /// Provides extension methods for <see cref="OrderItemAddRequest"/> objects.
 /// </summary>
 public static class OrderItemAddRequestExtensions
 {
  /// <summary>
  /// Converts a list of <see cref="OrderItemAddRequest"/> objects to a list of <see cref="OrderItem"/> objects.
  /// </summary>
  /// <param name="orderItemRequests">The list of <see cref="OrderItemAddRequest"/> objects to convert.</param>
  /// <returns>A list of <see cref="OrderItem"/> objects.</returns>
  public static List<OrderItem> ToOrderItems(this List<OrderItemAddRequest> orderItemRequests)
  {
   var orderItems = new List<OrderItem>();
   foreach (var orderItemRequest in orderItemRequests)
   {
    var orderItem = new OrderItem
    {
     OrderId = orderItemRequest.OrderId,
     ProductName = orderItemRequest.ProductName,
     Quantity = orderItemRequest.Quantity,
     UnitPrice = orderItemRequest.UnitPrice,
     TotalPrice = orderItemRequest.TotalPrice
    };

    orderItems.Add(orderItem);
   }

   return orderItems;
  }
 }

}
