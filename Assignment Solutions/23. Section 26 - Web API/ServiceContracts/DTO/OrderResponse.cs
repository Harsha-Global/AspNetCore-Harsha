using OrderManagement.Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
 /// <summary>
 /// Represents a response for an order.
 /// </summary>
 public class OrderResponse
 {
  /// <summary>
  /// Gets or sets the ID of the order.
  /// </summary>
  public Guid OrderId { get; set; }


  /// <summary>
  /// Gets or sets the order number.
  /// </summary>
  public string? OrderNumber { get; set; }


  /// <summary>
  /// Gets or sets the name of the customer associated with the order.
  /// </summary>
  public string? CustomerName { get; set; }


  /// <summary>
  /// Gets or sets the date of the order.
  /// </summary>
  public DateTime OrderDate { get; set; }


  /// <summary>
  /// Gets or sets the total amount of the order.
  /// </summary>
  public decimal TotalAmount { get; set; }


  /// <summary>
  /// Gets or sets the list of order items associated with the order.
  /// </summary>
  public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();


  /// <summary>
  /// Converts the <see cref="OrderResponse"/> object to an <see cref="Order"/> object.
  /// </summary>
  /// <returns>The converted <see cref="Order"/> object.</returns>
  public Order ToOrder()
  {
   return new Order
   {
    OrderId = OrderId,
    OrderNumber = OrderNumber,
    CustomerName = CustomerName,
    OrderDate = OrderDate,
    TotalAmount = TotalAmount,
   };
  }
 }


 /// <summary>
 /// Provides extension methods for <see cref="Order"/> objects.
 /// </summary>
 public static class OrderExtensions
 {
  /// <summary>
  /// Converts an <see cref="Order"/> object to an <see cref="OrderResponse"/> object.
  /// </summary>
  /// <param name="order">The <see cref="Order"/> object to convert.</param>
  /// <returns>An <see cref="OrderResponse"/> object.</returns>
  public static OrderResponse ToOrderResponse(this Order order)
  {
   return new OrderResponse
   {
    OrderId = order.OrderId,
    OrderNumber = order.OrderNumber,
    CustomerName = order.CustomerName,
    OrderDate = order.OrderDate,
    TotalAmount = order.TotalAmount,
   };
  }

  /// <summary>
  /// Converts a list of <see cref="Order"/> objects to a list of <see cref="OrderResponse"/> objects.
  /// </summary>
  /// <param name="orders">The list of <see cref="Order"/> objects to convert.</param>
  /// <returns>A list of <see cref="OrderResponse"/> objects.</returns>
  public static List<OrderResponse> ToOrderResponseList(this List<Order> orders)
  {
   var orderResponses = new List<OrderResponse>();
   foreach (var order in orders)
   {
    orderResponses.Add(order.ToOrderResponse());
   }
   return orderResponses;
  }
 }

}
