using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
 /// <summary>
 /// Represents a service for adding orders.
 /// </summary>
 public interface IOrdersAdderService
 {
  /// <summary>
  /// Adds a new order.
  /// </summary>
  /// <param name="orderRequest">The order details.</param>
  /// <returns>The added order.</returns>
  Task<OrderResponse> AddOrder(OrderAddRequest orderRequest);
 }
}

