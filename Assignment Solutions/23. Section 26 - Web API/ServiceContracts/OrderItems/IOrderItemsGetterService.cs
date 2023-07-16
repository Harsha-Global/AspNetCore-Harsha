using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
 /// <summary>
 /// Represents the service contract for retrieving order items.
 /// </summary>
 public interface IOrderItemsGetterService
 {
  /// <summary>
  /// Retrieves all order items.
  /// </summary>
  /// <returns>A list of order items.</returns>
  Task<List<OrderItemResponse>> GetAllOrderItems();

  /// <summary>
  /// Retrieves order items associated with a specific order ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <returns>A list of order items associated with the order ID.</returns>
  Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId);

  /// <summary>
  /// Retrieves an order item based on its order item ID.
  /// </summary>
  /// <param name="orderItemId">The ID of the order item.</param>
  /// <returns>The order item matching the order item ID, or null if not found.</returns>
  Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId);
 }
}

