using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
 [Route("[controller]/[action]")]
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
  public async Task<IActionResult> Login(LoginDTO loginDTO)
  {
   if (!ModelState.IsValid)
   {
    ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
    return View(loginDTO);
   }

   var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

   if (result.Succeeded)
   {
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
 }
}
