using OrderSolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace OrderSolution.Controllers
{
 public class OrdersController : Controller
 {
  //When request is received at path "/order"
  [Route("/order")]

  //bind only the desired properties of Order class, i.e. 'OrderDate', "InvoicePrice" and "Products"
  public IActionResult Index([Bind(nameof(Order.OrderDate), nameof(Order.InvoicePrice), nameof(Order.Products))] Order order)
  {
   //if there are any validation errors (as per data annotations)
   if (!ModelState.IsValid)
   {
    string messages = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

    //semd HTTP 400 response with error messages
    return BadRequest(messages);
   }

   //generate a random order number between 1 and 99999
   Random random = new Random();
   int randomOrderNumber = random.Next(1, 99999);

   //return HTTP 200 response with order number
   return Json(new { orderNumber = randomOrderNumber });
  }
 }
}

