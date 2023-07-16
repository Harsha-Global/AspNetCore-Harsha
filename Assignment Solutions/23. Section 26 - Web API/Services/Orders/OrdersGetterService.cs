using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
 /// <summary>
 /// Represents a service for retrieving orders.
 /// </summary>
 public class OrdersGetterService : IOrdersGetterService
 {
  private readonly IOrdersRepository _ordersRepository;
  private readonly IOrderItemsGetterService _orderItemsGetterService;
  private readonly ILogger<OrdersGetterService> _logger;

  public OrdersGetterService(IOrdersRepository ordersRepository, IOrderItemsGetterService orderItemsGetterService,  ILogger<OrdersGetterService> logger)
  {
   _ordersRepository = ordersRepository;
   _orderItemsGetterService = orderItemsGetterService;
   _logger = logger;
  }


  /// <inheritdoc/>
  public async Task<List<OrderResponse>> GetAllOrders()
  {
   _logger.LogInformation("Retrieving all orders");

   var orders = await _ordersRepository.GetAllOrders();
   var orderResponses = orders.ToOrderResponseList();
   foreach (var orderResponse in orderResponses )
   {
    orderResponse.OrderItems = await _orderItemsGetterService.GetOrderItemsByOrderId(orderResponse.OrderId);
   }

   _logger.LogInformation("All orders retrieved successfully");

   return orderResponses;
  }


  /// <inheritdoc/>
  public async Task<OrderResponse?> GetOrderByOrderId(Guid orderId)
  {
   _logger.LogInformation($"Retrieving order with ID: {orderId}");

   var order = await _ordersRepository.GetOrderByOrderId(orderId);
   var orderResponse = order?.ToOrderResponse();
   orderResponse.OrderItems = await _orderItemsGetterService.GetOrderItemsByOrderId(orderResponse.OrderId);

   if (orderResponse == null)
   {
    _logger.LogWarning($"Order with ID {orderId} not found");
   }
   else
   {
    _logger.LogInformation($"Order with ID {orderId} retrieved successfully");
   }

   return orderResponse;
  }
 }
}

