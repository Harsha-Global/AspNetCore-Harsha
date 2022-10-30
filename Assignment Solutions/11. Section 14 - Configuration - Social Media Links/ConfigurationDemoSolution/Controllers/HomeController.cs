using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationDemoSolution.Controllers
{
 public class HomeController : Controller
 {
  private readonly SocialMediaLinksOptions _socialMediaLinksOptions;

  public HomeController(IOptions<SocialMediaLinksOptions> socialMediaLinksOptions)
  {
   _socialMediaLinksOptions = socialMediaLinksOptions.Value;
  }

  [Route("/")]
  public IActionResult Index()
  {
   ViewBag.SocialMediaLinks = _socialMediaLinksOptions;
   return View();
  }
 }
}

