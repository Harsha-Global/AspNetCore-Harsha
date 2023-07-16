using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Entities;
using OrderManagement.RepositoryContracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderManagement.Repositories
{
 /// <summary>
 /// Repository implementation for managing orders.
 /// </summary>
 public class OrdersRepository : IOrdersRepository
 {
  private readonly ApplicationDbContext _db;
  private readonly ILogger<OrdersRepository> _logger;


  public OrdersRepository(ApplicationDbContext db, ILogger<OrdersRepository> logger)
  {
   _db = db;
   _logger = logger;
  }


  /// <inheritdoc />
  public async Task<Order> AddOrder(Order order)
  {
   _logger.LogInformation("Adding order to the database...");

   // Add the new order to the database context
   _db.Orders.Add(order);

   // Save the changes to the database
   await _db.SaveChangesAsync();

   _logger.LogInformation("Order added successfully.");

   return order;
  }


  /// <inheritdoc />
  public async Task<bool> DeleteOrderByOrderId(Guid orderId)
  {
   _logger.LogInformation($"Deleting order with ID: {orderId}...");

   // Find the order with the specified ID
   var order = await _db.Orders.FindAsync(orderId);
   if (order == null)
   {
    _logger.LogWarning($"Order not found with ID: {orderId}.");
    return false;
   }

   // Remove the order from the database context
   _db.Orders.Remove(order);

   // Save the changes to the database
   await _db.SaveChangesAsync();

   _logger.LogInformation($"Order deleted successfully. ID: {orderId}.");

   return true;
  }


  /// <inheritdoc />
  public async Task<List<Order>> GetAllOrders()
  {
   _logger.LogInformation("Retrieving all orders...");

   // Retrieve all orders from the database, ordered by the order date in descending order
   var orders = await _db.Orders.OrderByDescending(temp => temp.OrderDate).ToListAsync();

   _logger.LogInformation($"Retrieved {orders.Count} orders successfully.");

   return orders;
  }


  /// <inheritdoc />
  public async Task<List<Order>> GetFilteredOrders(Expression<Func<Order, bool>> predicate)
  {
   _logger.LogInformation("Retrieving filtered orders...");

   // Retrieve filtered orders based on the specified predicate, ordered by the order date in descending order
   var filteredOrders = await _db.Orders.Where(predicate)
       .OrderByDescending(temp => temp.OrderDate).ToListAsync();

   _logger.LogInformation($"Retrieved {filteredOrders.Count} filtered orders successfully.");

   return filteredOrders;
  }


  /// <inheritdoc />
  public async Task<Order?> GetOrderByOrderId(Guid orderId)
  {
   _logger.LogInformation($"Retrieving order with ID: {orderId}...");

   // Find the order with the specified ID
   var order = await _db.Orders.FindAsync(orderId);

   if (order == null)
   {
    _logger.LogWarning($"Order not found with ID: {orderId}.");
   }
   else
   {
    _logger.LogInformation($"Order retrieved successfully. ID: {orderId}.");
   }

   return order;
  }


  /// <inheritdoc />
  public async Task<Order> UpdateOrder(Order order)
  {
   _logger.LogInformation($"Updating order with ID: {order.OrderId}...");

   // Find the existing order in the database
   var existingOrder = await _db.Orders.FindAsync(order.OrderId);
   if (existingOrder == null)
   {
    _logger.LogWarning($"Order not found with ID: {order.OrderId}.");
    return order;
   }

   // Update the properties of the existing order with the new values
   existingOrder.OrderNumber = order.OrderNumber;
   existingOrder.OrderDate = order.OrderDate;
   existingOrder.CustomerName = order.CustomerName;
   existingOrder.TotalAmount = order.TotalAmount;

   // Save the changes to the database
   await _db.SaveChangesAsync();

   _logger.LogInformation($"Order updated successfully. ID: {order.OrderId}.");

   return order;
  }
 }
}
