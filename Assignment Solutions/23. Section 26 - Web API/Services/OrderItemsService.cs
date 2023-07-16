using Microsoft.Extensions.Logging;
using OrderManagement.Entities;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
 public class OrderItemsService : IOrderItemsService
 {
  private readonly IOrderItemsRepository _orderItemsRepository;
  private readonly ILogger<OrderItemsService> _logger;

  public OrderItemsService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsService> logger)
  {
   _orderItemsRepository = orderItemsRepository;
   _logger = logger;
  }


  public async Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest)
  {
   var orderItem = orderItemRequest.ToOrderItem();
   orderItem.OrderItemId = Guid.NewGuid();

   var addedOrderItem = await _orderItemsRepository.AddOrderItem(orderItem);
   return addedOrderItem.ToOrderItemResponse();
  }


  public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
  {
   return await _orderItemsRepository.DeleteOrderItemByOrderItemId(orderItemId);
  }


  public async Task<List<OrderItemResponse>> GetAllOrderItems()
  {
   var orderItems = await _orderItemsRepository.GetAllOrderItems();
   return orderItems.ToOrderItemResponseList();
  }


  public async Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
  {
   var orderItems = await _orderItemsRepository.GetOrderItemsByOrderId(orderId);
   return orderItems.ToOrderItemResponseList();
  }


  public async Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
  {
   var orderItem = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemId);
   return orderItem?.ToOrderItemResponse();
  }


  public async Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest)
  {
   var orderItem = orderItemRequest.ToOrderItem();

   var updatedOrderItem = await _orderItemsRepository.UpdateOrderItem(orderItem);
   return updatedOrderItem.ToOrderItemResponse();
  }
 }
}
