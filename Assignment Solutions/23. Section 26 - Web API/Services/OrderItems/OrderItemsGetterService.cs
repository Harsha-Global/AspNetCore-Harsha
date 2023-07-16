using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using OrderManagement.ServiceContracts;
using OrderManagement.RepositoryContracts;

namespace OrderManagement.Services
{
 /// <summary>
 /// Service class for getting order items.
 /// </summary>
 public class OrderItemsGetterService : IOrderItemsGetterService
 {
  private readonly IOrderItemsRepository _orderItemsRepository;
  private readonly ILogger<OrderItemsGetterService> _logger;


  public OrderItemsGetterService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsGetterService> logger)
  {
   _orderItemsRepository = orderItemsRepository;
   _logger = logger;
  }


  /// <inheritdoc />
  public async Task<List<OrderItemResponse>> GetAllOrderItems()
  {
   _logger.LogInformation("Retrieving all order items...");

   // Retrieve all order items from the repository
   var orderItems = await _orderItemsRepository.GetAllOrderItems();

   _logger.LogInformation("All order items retrieved successfully.");

   // Convert the order items to response DTOs
   return orderItems.ToOrderItemResponseList();
  }


  /// <inheritdoc />
  public async Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
  {
   _logger.LogInformation($"Retrieving order items for Order ID: {orderId}...");

   // Retrieve order items associated with the specified Order ID
   var orderItems = await _orderItemsRepository.GetOrderItemsByOrderId(orderId);

   _logger.LogInformation($"Order items retrieved successfully for Order ID: {orderId}.");

   // Convert the order items to response DTOs
   return orderItems.ToOrderItemResponseList();
  }


  /// <inheritdoc />
  public async Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
  {
   _logger.LogInformation($"Retrieving order item by Order Item ID: {orderItemId}...");

   // Retrieve an order item based on its Order Item ID
   var orderItem = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemId);

   if (orderItem == null)
   {
    _logger.LogWarning($"Order item not found for Order Item ID: {orderItemId}.");
   }
   else
   {
    _logger.LogInformation($"Order item retrieved successfully. Order Item ID: {orderItemId}.");
   }

   // Convert the order item to a response DTO
   return orderItem?.ToOrderItemResponse();
  }
 }
}
