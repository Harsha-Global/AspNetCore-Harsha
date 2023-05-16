using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1
{
 /// <summary>
 /// 
 /// </summary>
 [AllowAnonymous]
 [ApiVersion("1.0")]
 public class AccountController : CustomControllerBase
 {
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<ApplicationRole> _roleManager;
  private readonly IJwtService _jwtService;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="userManager"></param>
  /// <param name="signInManager"></param>
  /// <param name="roleManager"></param>
  public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
  {
   _userManager = userManager;
   _signInManager = signInManager;
   _roleManager = roleManager;
   _jwtService = jwtService;
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="registerDTO"></param>
  /// <returns></returns>
  [HttpPost("register")]
  public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
  {
   //Validation
   if (ModelState.IsValid == false)
   {
    string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
    return Problem(errorMessage);
   }


   //Create user
   ApplicationUser user = new ApplicationUser()
   {
    Email = registerDTO.Email,
    PhoneNumber = registerDTO.PhoneNumber,
    UserName = registerDTO.Email,
    PersonName = registerDTO.PersonName
   };

   IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

   if (result.Succeeded)
   {
    //sign-in
    await _signInManager.SignInAsync(user, isPersistent: false);

    var authenticationResponse = _jwtService.CreateJwtToken(user);

    return Ok(authenticationResponse);
   }
   else
   {
    string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description)); //error1 | error2
    return Problem(errorMessage);
   }
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="email"></param>
  /// <returns></returns>
  [HttpGet]
  public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
  {
   ApplicationUser? user = await _userManager.FindByEmailAsync(email);

   if (user == null)
   {
    return Ok(true);
   }
   else
   {
    return Ok(false);
   }
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="loginDTO"></param>
  /// <returns></returns>
  [HttpPost("login")]
  public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
  {
   //Validation
   if (ModelState.IsValid == false)
   {
    string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
    return Problem(errorMessage);
   }


   var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

   if (result.Succeeded)
   {
    ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

    if (user == null)
    {
     return NoContent();
    }

    //sign-in
    await _signInManager.SignInAsync(user, isPersistent: false);

    var authenticationResponse = _jwtService.CreateJwtToken(user);

    return Ok(authenticationResponse);
   }

   else
   {
    return Problem("Invalid email or password");
   }
  }


  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  [HttpGet("logout")]
  public async Task<IActionResult> GetLogout()
  {
   await _signInManager.SignOutAsync();

   return NoContent();
  }
 }
}
