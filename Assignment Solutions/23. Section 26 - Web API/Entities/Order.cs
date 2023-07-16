using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Entities
{
 /// <summary>
 /// Represents an order entity.
 /// </summary>
 public class Order
 {
  /// <summary>
  /// Gets or sets the unique identifier of the order.
  /// </summary>
  [Key]
  public Guid OrderId { get; set; }


  /// <summary>
  /// Gets or sets the name of the customer associated with the order.
  /// </summary>
  [Required(ErrorMessage = "The CustomerName field is required.")]
  [StringLength(50, ErrorMessage = "The CustomerName field must not exceed 50 characters.")]
  public string? CustomerName { get; set; }


  /// <summary>
  /// Gets or sets the order number.
  /// </summary>
  [Required(ErrorMessage = "The OrderNumber field is required.")]
  [RegularExpression(@"^(?i)ORD_\d{4}_\d+$\r\n", ErrorMessage = "The Order number should begin with 'ORD' followed by an underscore (_) and a sequential number.")]
  public string? OrderNumber { get; set; }


  /// <summary>
  /// Gets or sets the date of the order.
  /// </summary>
  [Required(ErrorMessage = "The OrderDate field is required.")]
  public DateTime OrderDate { get; set; }


  /// <summary>
  /// Gets or sets the total amount of the order.
  /// </summary>
  [Range(0, double.MaxValue, ErrorMessage = "The TotalAmount field must be a positive number.")]
  [Column(TypeName = "decimal")]
  public decimal TotalAmount { get; set; }
 }
}

