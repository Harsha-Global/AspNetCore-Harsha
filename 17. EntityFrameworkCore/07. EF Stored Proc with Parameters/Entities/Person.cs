using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
  /// <summary>
  /// Person domain model class
  /// </summary>
  public class Person
  {
    [Key]
    public Guid PersonID { get; set; }

    [StringLength(40)] //nvarchar(40)
    public string? PersonName { get; set; }

    [StringLength(40)] //nvarchar(40)
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)] //nvarchar(100)
    public string? Gender { get; set; }

    //uniqueidentifier
    public Guid? CountryID { get; set; }

    [StringLength(200)] //nvarchar(200)
    public string? Address { get; set; }

    //bit
    public bool ReceiveNewsLetters { get; set; }

  }
}
