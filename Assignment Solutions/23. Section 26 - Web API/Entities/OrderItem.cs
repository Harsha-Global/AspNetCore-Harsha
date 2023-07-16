using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Entities
{
 /// <summary>
 /// Represents an item within an order.
 /// </summary>
 public class OrderItem
 {
  /// <summary>
  /// Gets or sets the unique identifier of the order item.
  /// </summary>
  [Key]
  public Guid OrderItemId { get; set; }


  /// <summary>
  /// Gets or sets the unique identifier of the order associated with the item.
  /// </summary>
  [Required(ErrorMessage = "The OrderId field is required.")]
  public Guid OrderId { get; set; }


  /// <summary>
  /// Gets or sets the name of the product associated with the order item.
  /// </summary>
  [Required(ErrorMessage = "The ProductName field is required.")]
  [StringLength(50, ErrorMessage = "The ProductName field must not exceed 50 characters.")]
  public string? ProductName { get; set; }


  /// <summary>
  /// Gets or sets the quantity of the product in the order item.
  /// </summary>
  [Range(1, int.MaxValue, ErrorMessage = "The Quantity field must be a positive number.")]
  public int Quantity { get; set; }


  /// <summary>
  /// Gets or sets the unit price of the product in the order item.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The UnitPrice field must be a positive number.")]
  [Column(TypeName = "decimal")]
  public decimal UnitPrice { get; set; }


  /// <summary>
  /// Gets or sets the total price of the order item.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The total price of the order item.")]
  [Column(TypeName = "decimal")]
  public decimal TotalPrice { get; set; }
 }
}

