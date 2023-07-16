using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
 /// <summary>
 /// Service class for deleting order items.
 /// </summary>
 public class OrderItemsDeleterService : IOrderItemsDeleterService
 {
  private readonly IOrderItemsRepository _orderItemsRepository;
  private readonly ILogger<OrderItemsDeleterService> _logger;


  /// <summary>
  /// Initializes a new instance of the <see cref="OrderItemsDeleterService"/> class.
  /// </summary>
  /// <param name="orderItemsRepository">The repository for order items.</param>
  /// <param name="logger">The logger.</param>
  public OrderItemsDeleterService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsDeleterService> logger)
  {
   _orderItemsRepository = orderItemsRepository;
   _logger = logger;
  }


  /// <inheritdoc />
  public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
  {
   _logger.LogInformation($"Deleting order item with ID: {orderItemId}...");

   // Delete the order item by its Order Item ID
   var isDeleted = await _orderItemsRepository.DeleteOrderItemByOrderItemId(orderItemId);

   if (isDeleted)
   {
    _logger.LogInformation($"Order item deleted successfully. ID: {orderItemId}.");
   }
   else
   {
    _logger.LogWarning($"Order item not found for deletion. ID: {orderItemId}.");
   }

   return isDeleted;
  }
 }
}

