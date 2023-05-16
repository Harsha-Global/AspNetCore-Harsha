using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1
{
 [AllowAnonymous]
 [ApiVersion("1.0")]
 public class AccountController : CustomControllerBase
 {
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<ApplicationRole> _roleManager;


  public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
  {
   _userManager = userManager;
   _signInManager = signInManager;
   _roleManager = roleManager;
  }


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

    return Ok(user);
   }
   else
   {
    string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description)); //error1 | error2
    return Problem(errorMessage);
   }
  }


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
 }
}
