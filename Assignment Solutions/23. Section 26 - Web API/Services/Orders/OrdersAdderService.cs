using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
 /// <summary>
 /// Represents a service for adding orders.
 /// </summary>
 public class OrdersAdderService : IOrdersAdderService
 {
  private readonly IOrdersRepository _ordersRepository;
  private readonly IOrderItemsRepository _orderItemsRepository;
  private readonly ILogger<OrdersAdderService> _logger;


  /// <summary>
  /// Initializes a new instance of the <see cref="OrdersAdderService"/> class.
  /// </summary>
  /// <param name="ordersRepository">The repository for orders.</param>
  /// <param name="orderItemsRepository">The repository for order items.</param>
  /// <param name="logger">The logger instance.</param>
  public OrdersAdderService(IOrdersRepository ordersRepository, IOrderItemsRepository orderItemsRepository, ILogger<OrdersAdderService> logger)
  {
   _ordersRepository = ordersRepository;
   _orderItemsRepository = orderItemsRepository;
   _logger = logger;
  }


  /// <inheritdoc/>
  public async Task<OrderResponse> AddOrder(OrderAddRequest orderRequest)
  {
   _logger.LogInformation("Adding a new order");

   // Create a new order entity and generate a unique OrderId
   var order = orderRequest.ToOrder();
   order.OrderId = Guid.NewGuid();

   // Add the order to the repository
   var addedOrder = await _ordersRepository.AddOrder(order);
   var addedOrderResponse = addedOrder.ToOrderResponse();

   // Add the order items to the repository and associate them with the order
   foreach (var item in orderRequest.OrderItems)
   {
    var orderItem = item.ToOrderItem();
    orderItem.OrderItemId = Guid.NewGuid();
    orderItem.OrderId = addedOrder.OrderId;

    var addedOrderItem = await _orderItemsRepository.AddOrderItem(orderItem);
    addedOrderResponse.OrderItems.Add(addedOrderItem.ToOrderItemResponse());
   }

   _logger.LogInformation("Order added successfully");

   return addedOrderResponse;
  }
 }
}

