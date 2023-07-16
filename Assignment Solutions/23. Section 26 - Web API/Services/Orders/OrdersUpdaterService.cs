using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
 /// <summary>
 /// Represents a service for updating orders.
 /// </summary>
 public class OrdersUpdaterService : IOrdersUpdaterService
 {
  private readonly IOrdersRepository _ordersRepository;
  private readonly ILogger<OrdersUpdaterService> _logger;


  public OrdersUpdaterService(IOrdersRepository ordersRepository, ILogger<OrdersUpdaterService> logger)
  {
   _ordersRepository = ordersRepository;
   _logger = logger;
  }


  /// <inheritdoc/>
  public async Task<OrderResponse> UpdateOrder(OrderUpdateRequest orderRequest)
  {
   _logger.LogInformation($"Updating order with ID: {orderRequest.OrderId}");

   var order = orderRequest.ToOrder();
   var updatedOrder = await _ordersRepository.UpdateOrder(order);
   var updatedOrderResponse = updatedOrder.ToOrderResponse();

   _logger.LogInformation($"Order with ID {orderRequest.OrderId} updated successfully");

   return updatedOrderResponse;
  }
 }
}

