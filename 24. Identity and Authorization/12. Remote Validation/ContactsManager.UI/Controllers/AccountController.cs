using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
 [Route("[controller]/[action]")]
 [AllowAnonymous]
 public class AccountController : Controller
 {
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;

  public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
  {
   _userManager = userManager;
   _signInManager = signInManager;
  }


  [HttpGet]
  public IActionResult Register()
  {
   return View();
  }


  [HttpPost]
  public async Task<IActionResult> Register(RegisterDTO registerDTO)
  {
   //Check for validation errors
   if (ModelState.IsValid == false)
   {
    ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
    return View(registerDTO);
   }

   ApplicationUser user = new ApplicationUser() { Email = registerDTO.Email, PhoneNumber = registerDTO.Phone, UserName = registerDTO.Email, PersonName = registerDTO.PersonName };

   IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
   if (result.Succeeded)
   {
    //Sign in
    await _signInManager.SignInAsync(user, isPersistent: false);

    return RedirectToAction(nameof(PersonsController.Index), "Persons");
   }
   else
   {
    foreach (IdentityError error in result.Errors)
    {
     ModelState.AddModelError("Register", error.Description);
    }

    return View(registerDTO);
   }
   
  }


  [HttpGet]
  public IActionResult Login()
  {
   return View();
  }


  [HttpPost]
  public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
  {
   if (!ModelState.IsValid)
   {
    ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
    return View(loginDTO);
   }

   var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

   if (result.Succeeded)
   {
    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
    {
     return LocalRedirect(ReturnUrl);
    }
    return RedirectToAction(nameof(PersonsController.Index), "Persons");
   }

   ModelState.AddModelError("Login", "Inalid email or password");
   return View(loginDTO);
  }


  public async Task<IActionResult> Logout()
  {
   await _signInManager.SignOutAsync();
   return RedirectToAction(nameof(PersonsController.Index), "Persons");
  }


  public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
  {
   ApplicationUser user = await _userManager.FindByEmailAsync(email);
   if (user == null)
   {
    return Json(true); //valid
   }
   else
   {
    return Json(false); //invalid
   }
  }
 }
}
