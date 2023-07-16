using OrderManagement.Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a response for an order item.
 /// </summary>
 public class OrderItemResponse
 {
  /// <summary>
  /// Gets or sets the ID of the order item.
  /// </summary>
  public Guid OrderItemId { get; set; }


  /// <summary>
  /// Gets or sets the ID of the order associated with the order item.
  /// </summary>
  public Guid OrderId { get; set; }


  /// <summary>
  /// Gets or sets the name of the product associated with the order item.
  /// </summary>
  public string? ProductName { get; set; }


  /// <summary>
  /// Gets or sets the quantity of the product in the order item.
  /// </summary>
  public int Quantity { get; set; }


  /// <summary>
  /// Gets or sets the unit price of the product in the order item.
  /// </summary>
  public decimal UnitPrice { get; set; }


  /// <summary>
  /// Gets or sets the total price of the order item.
  /// </summary>
  public decimal TotalPrice { get; set; }
 }


 /// <summary>
 /// Provides extension methods for <see cref="OrderItem"/> objects.
 /// </summary>
 public static class OrderItemResponseExtensions
 {
  /// <summary>
  /// Converts an <see cref="OrderItem"/> object to an <see cref="OrderItemResponse"/> object.
  /// </summary>
  /// <param name="orderItem">The <see cref="OrderItem"/> object to convert.</param>
  /// <returns>An <see cref="OrderItemResponse"/> object.</returns>
  public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
  {
   return new OrderItemResponse
   {
    OrderItemId = orderItem.OrderItemId,
    OrderId = orderItem.OrderId,
    ProductName = orderItem.ProductName,
    Quantity = orderItem.Quantity,
    UnitPrice = orderItem.UnitPrice,
    TotalPrice = orderItem.TotalPrice
   };
  }


  /// <summary>
  /// Converts a list of <see cref="OrderItem"/> objects to a list of <see cref="OrderItemResponse"/> objects.
  /// </summary>
  /// <param name="orderItems">The list of <see cref="OrderItem"/> objects to convert.</param>
  /// <returns>A list of <see cref="OrderItemResponse"/> objects.</returns>
  public static List<OrderItemResponse> ToOrderItemResponseList(this List<OrderItem> orderItems)
  {
   var orderItemResponses = new List<OrderItemResponse>();
   foreach (var orderItem in orderItems)
   {
    orderItemResponses.Add(orderItem.ToOrderItemResponse());
   }
   return orderItemResponses;
  }
 }
}

