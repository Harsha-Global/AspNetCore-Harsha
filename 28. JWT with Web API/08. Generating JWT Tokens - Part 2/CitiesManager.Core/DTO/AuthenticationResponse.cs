using System;

namespace CitiesManager.Core.DTO
{
 public class AuthenticationResponse
 {
  public string? PersonName { get; set; } = string.Empty;
  public string? Email { get; set; } = string.Empty;
  public string? Token { get; set; } = string.Empty;
  public DateTime Expiration { get; set; }
 }
}
