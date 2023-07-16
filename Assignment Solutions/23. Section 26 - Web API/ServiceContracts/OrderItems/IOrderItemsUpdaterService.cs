using System;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
 /// <summary>
 /// Represents the service contract for updating order items.
 /// </summary>
 public interface IOrderItemsUpdaterService
 {
  /// <summary>
  /// Updates an order item.
  /// </summary>
  /// <param name="orderItemRequest">The request containing the updated order item data.</param>
  /// <returns>The updated order item.</returns>
  Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest);
 }
}

