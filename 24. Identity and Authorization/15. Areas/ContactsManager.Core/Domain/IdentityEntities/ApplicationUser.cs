using Microsoft.AspNetCore.Identity;
using System;

namespace ContactsManager.Core.Domain.IdentityEntities
{
 public class ApplicationUser : IdentityUser<Guid>
 {
  public string? PersonName { get; set; }
 }
}
