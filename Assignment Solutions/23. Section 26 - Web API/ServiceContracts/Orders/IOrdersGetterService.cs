using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
 /// <summary>
 /// Represents a service for retrieving orders.
 /// </summary>
 public interface IOrdersGetterService
 {
  /// <summary>
  /// Retrieves all orders.
  /// </summary>
  /// <returns>A list of orders.</returns>
  Task<List<OrderResponse>> GetAllOrders();

  /// <summary>
  /// Retrieves an order by its ID.
  /// </summary>
  /// <param name="orderId">The ID of the order to retrieve.</param>
  /// <returns>The retrieved order, or null if not found.</returns>
  Task<OrderResponse?> GetOrderByOrderId(Guid orderId);
 }
}

