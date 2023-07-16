using System;
using System.Threading.Tasks;

namespace OrderManagement.ServiceContracts
{
 /// <summary>
 /// Represents the service contract for deleting order items.
 /// </summary>
 public interface IOrderItemsDeleterService
 {
  /// <summary>
  /// Deletes an order item based on its order item ID.
  /// </summary>
  /// <param name="orderItemId">The ID of the order item to delete.</param>
  /// <returns>A boolean indicating whether the deletion was successful.</returns>
  Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId);
 }
}

