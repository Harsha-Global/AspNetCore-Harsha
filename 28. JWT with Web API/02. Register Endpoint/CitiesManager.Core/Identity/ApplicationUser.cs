using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Core.Identity
{
 public class ApplicationUser : IdentityUser<Guid>
 {
  public string? PersonName { get; set; }
 }
}

