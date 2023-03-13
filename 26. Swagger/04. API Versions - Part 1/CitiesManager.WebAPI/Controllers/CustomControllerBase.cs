using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
 [Route("api/v{version:apiVersion}/[controller]")]
 [ApiController]
 public class CustomControllerBase : ControllerBase
 {
 }
}

