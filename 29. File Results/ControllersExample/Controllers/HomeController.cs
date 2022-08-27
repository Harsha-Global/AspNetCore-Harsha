using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("home")]
    [Route("/")]
    public ContentResult Index()
    {
      //return new ContentResult() { Content = "Hello from Index", ContentType = "text/plain" };

      //return Content("Hello from Index", "text/plain");

      return Content("<h1>Welcome</h1> <h2>Hello from Index</h2>", "text/html");
    }

    [Route("person")]
    public JsonResult Person()
    {
      Person person = new Person() { Id = Guid.NewGuid(), FirstName = "James", LastName = "Smith", Age = 25 };
      //return new JsonResult(person);
      return Json(person);
      //return "{ \"key\": \"value\" }";
    }

    [Route("file-download")]
    public VirtualFileResult FileDownload()
    {
      //return new VirtualFileResult("/sample.pdf", "application/pdf");
      return File("/sample.pdf", "application/pdf");
    }

    [Route("file-download2")]
    public PhysicalFileResult FileDownload2()
    {
      //return new PhysicalFileResult(@"c:\aspnetcore\sample.pdf", "application/pdf");
      return PhysicalFile(@"c:\aspnetcore\sample.pdf", "application/pdf");
    }

    [Route("file-download3")]
    public FileContentResult FileDownload3()
    {
      byte[] bytes = System.IO.File.ReadAllBytes(@"c:\aspnetcore\sample.pdf");
      //return new FileContentResult(bytes, "application/pdf");
      return File(bytes,"application/pdf");
    }
  }
}
