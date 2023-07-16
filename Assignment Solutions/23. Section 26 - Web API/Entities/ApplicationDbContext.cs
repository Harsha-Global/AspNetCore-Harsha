using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace OrderManagement.Entities
{
 /// <summary>
 /// Represents the database context for the application.
 /// </summary>
 public class ApplicationDbContext : DbContext
 {
  /// <summary>
  /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
  /// </summary>
  /// <param name="options">The options for configuring the context.</param>
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  /// <summary>
  /// Gets or sets the DbSet for orders.
  /// </summary>
  public DbSet<Order> Orders { get; set; }


  /// <summary>
  /// Gets or sets the DbSet for order items.
  /// </summary>
  public DbSet<OrderItem> OrderItems { get; set; }


  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   // Map the Order entity to the "Orders" table
   modelBuilder.Entity<Order>().ToTable("Orders");

   // Map the OrderItem entity to the "OrderItems" table
   modelBuilder.Entity<OrderItem>().ToTable("OrderItems");


   // Example seed data for Orders
   modelBuilder.Entity<Order>().HasData(
       new Order { OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), OrderNumber = "ORD001", CustomerName = "John Doe", OrderDate = DateTime.Now, TotalAmount = 66.5m },
       new Order { OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), OrderNumber = "ORD002", CustomerName = "Jane Smith", OrderDate = DateTime.Now, TotalAmount = 225.8m }
   );

   // Example seed data for OrderItems
   modelBuilder.Entity<OrderItem>().HasData(
       new OrderItem { OrderItemId = Guid.Parse("D20882DF-7FCA-4EE8-88BB-37D2FC75E63F"), OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), ProductName = "Product A", Quantity = 2, UnitPrice = 10.00m, TotalPrice = 20.00m },
       new OrderItem { OrderItemId = Guid.Parse("2E27B6A4-469D-4D7F-8B8B-54AF129675FD"), OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), ProductName = "Product B", Quantity = 3, UnitPrice = 15.50m, TotalPrice = 46.50m },
       new OrderItem { OrderItemId = Guid.Parse("24D71AC2-0A9C-4914-9FD3-13BC25D42694"), OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), ProductName = "Product C", Quantity = 7, UnitPrice = 25.40m, TotalPrice = 25.00m },
       new OrderItem { OrderItemId = Guid.Parse("AC90B8BC-349D-43FD-87A6-6A7ED8057697"), OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), ProductName = "Product D", Quantity = 4, UnitPrice = 12.00m, TotalPrice = 25.00m }
   );
  }
 }
}
