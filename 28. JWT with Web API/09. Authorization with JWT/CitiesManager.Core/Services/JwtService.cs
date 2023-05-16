using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Core.Services
{
 public class JwtService : IJwtService
 {
  private readonly IConfiguration _configuration;

  public JwtService(IConfiguration configuration)
  {
   _configuration = configuration;
  }


  /// <summary>
  /// Generates a JWT token using the given user's information and the configuration settings.
  /// </summary>
  /// <param name="user">ApplicationUser object</param>
  /// <returns>AuthenticationResponse that includes token</returns>
  public AuthenticationResponse CreateJwtToken(ApplicationUser user)
  {
   // Create a DateTime object representing the token expiration time by adding the number of minutes specified in the configuration to the current UTC time.
   DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

   // Create an array of Claim objects representing the user's claims, such as their ID, name, email, etc.
   Claim[] claims = new Claim[] {
     new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (user id)
     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID
     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), //Issued at (date and time of token generation)
     new Claim(ClaimTypes.NameIdentifier, user.Email), //Unique name identifier of the user (Email)
     new Claim(ClaimTypes.Name, user.PersonName) //Name of the user
     };

   // Create a SymmetricSecurityKey object using the key specified in the configuration.
   SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

   // Create a SigningCredentials object with the security key and the HMACSHA256 algorithm.
   SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

   // Create a JwtSecurityToken object with the given issuer, audience, claims, expiration, and signing credentials.
   JwtSecurityToken tokenGenerator = new JwtSecurityToken(
   _configuration["Jwt:Issuer"],
   _configuration["Jwt:Audience"],
   claims,
   expires: expiration,
   signingCredentials: signingCredentials
   );

   // Create a JwtSecurityTokenHandler object and use it to write the token as a string.
   JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
   string token = tokenHandler.WriteToken(tokenGenerator);

   // Create and return an AuthenticationResponse object containing the token, user email, user name, and token expiration time.
   return new AuthenticationResponse() { Token = token, Email = user.Email, PersonName = user.PersonName, Expiration = expiration };
  }
 }
}
