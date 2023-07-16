using OrderManagement.Entities;

namespace OrderManagement.RepositoryContracts
{
 /// <summary>
 /// Represents the repository contract for order items.
 /// </summary>
 public interface IOrderItemsRepository
 {
  /// <summary>
  /// Adds an order item to the repository.
  /// </summary>
  /// <param name="orderItem">The order item to add.</param>
  /// <returns>The added order item.</returns>
  Task<OrderItem> AddOrderItem(OrderItem orderItem);


  /// <summary>
  /// Deletes an order item from the repository based on its order item ID.
  /// </summary>
  /// <param name="orderItemId">The ID of the order item to delete.</param>
  /// <returns>A boolean indicating whether the deletion was successful.</returns>
  Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId);


  /// <summary>
  /// Retrieves all order items from the repository.
  /// </summary>
  /// <returns>A list of order items.</returns>
  Task<List<OrderItem>> GetAllOrderItems();


  /// <summary>
  /// Retrieves the order items associated with a specific order ID from the repository.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <returns>A list of order items associated with the order ID.</returns>
  Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId);


  /// <summary>
  /// Retrieves an order item from the repository based on its order item ID.
  /// </summary>
  /// <param name="orderItemId">The ID of the order item.</param>
  /// <returns>The order item matching the order item ID, or null if not found.</returns>
  Task<OrderItem?> GetOrderItemByOrderItemId(Guid orderItemId);


  /// <summary>
  /// Updates an order item in the repository.
  /// </summary>
  /// <param name="orderItem">The updated order item.</param>
  /// <returns>The updated order item.</returns>
  Task<OrderItem> UpdateOrderItem(OrderItem orderItem);
 }
}
