using OrderManagement.Entities;
using System.Linq.Expressions;

namespace OrderManagement.RepositoryContracts
{
 /// <summary>
 /// Represents the repository contract for orders.
 /// </summary>
 public interface IOrdersRepository
 {
  /// <summary>
  /// Adds an order to the repository.
  /// </summary>
  /// <param name="order">The order to add.</param>
  /// <returns>The added order.</returns>
  Task<Order> AddOrder(Order order);


  /// <summary>
  /// Deletes an order from the repository based on its order ID.
  /// </summary>
  /// <param name="orderId">The ID of the order to delete.</param>
  /// <returns>A boolean indicating whether the deletion was successful.</returns>
  Task<bool> DeleteOrderByOrderId(Guid orderId);


  /// <summary>
  /// Retrieves all orders from the repository.
  /// </summary>
  /// <returns>A list of orders.</returns>
  Task<List<Order>> GetAllOrders();


  /// <summary>
  /// Retrieves filtered orders from the repository based on the specified predicate.
  /// </summary>
  /// <param name="predicate">The predicate used to filter the orders.</param>
  /// <returns>A list of filtered orders.</returns>
  Task<List<Order>> GetFilteredOrders(Expression<Func<Order, bool>> predicate);


  /// <summary>
  /// Retrieves an order from the repository based on its order ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <returns>The order matching the order ID, or null if not found.</returns>
  Task<Order?> GetOrderByOrderId(Guid orderId);


  /// <summary>
  /// Updates an order in the repository.
  /// </summary>
  /// <param name="order">The updated order.</param>
  /// <returns>The updated order.</returns>
  Task<Order> UpdateOrder(Order order);
 }
}
