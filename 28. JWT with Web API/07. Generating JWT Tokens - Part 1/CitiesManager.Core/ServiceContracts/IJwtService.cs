using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using System;

namespace CitiesManager.Core.ServiceContracts
{
 public interface IJwtService
 {
  AuthenticationResponse CreateJwtToken(ApplicationUser user);
 }
}
