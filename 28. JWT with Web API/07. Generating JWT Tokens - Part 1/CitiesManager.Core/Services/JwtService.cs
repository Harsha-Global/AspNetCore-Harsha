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

  public JwtService(IConfiguration configuration) { 
   _configuration = configuration;
  }


  public AuthenticationResponse CreateJwtToken(ApplicationUser user)
  {
   DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

   Claim[] claims = new Claim[] {
    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (user id)

    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID

    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), //Issued at (date and time of token generation)

    new Claim(ClaimTypes.NameIdentifier, user.Email), //Unique name identifier of the user (Email)

    new Claim(ClaimTypes.Name, user.PersonName) //Name of the user
   };


   SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
    );

   SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

   JwtSecurityToken tokenGenerator = new JwtSecurityToken(
    _configuration["Jwt:Issuer"],
    _configuration["Jwt:Audience"],
    claims,
    expires: expiration,
    signingCredentials: signingCredentials
    );

   JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
   string token = tokenHandler.WriteToken(tokenGenerator);


   return new AuthenticationResponse() { Token = token, Email = user.Email, PersonName = user.PersonName, Expiration = expiration };

  }
 }
}
