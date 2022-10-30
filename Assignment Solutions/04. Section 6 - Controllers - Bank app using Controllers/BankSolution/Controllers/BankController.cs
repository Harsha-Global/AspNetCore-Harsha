using Microsoft.AspNetCore.Mvc;

namespace BankSolution.Controllers
{
 public class BankController : Controller
 {
  //When request is received at path "/"
  [Route("/")]
  public IActionResult Index()
  {
   return Content("Welcome to the Best Bank");
  }

  //When request is received at path "/account-details"
  [Route("/account-details")]
  public IActionResult AccountDetails()
  {
   //hard-coded data
   var bankAccount = new { accountNumber = 1001, accountHolderName = "Example Name", currentBalance = 5000 };

   //send the object as JSON
   return Json(bankAccount);
  }

  //When request is received at path "/account-statement"
  [Route("/account-statement")]
  public IActionResult AccountStatement()
  {
   //send a pdf file (at wwwroot folder) as response
   return File("~/statement.pdf", "application/pdf");
  }

  //When request is received at path "/get-current-balance/{accountNumber}"
  [Route("/get-current-balance/{accountNumber:int?}")]
  public IActionResult GetCurrentBalance(int? accountNumber)
  {
   //hard-coded data
   var bankAccount = new { accountNumber = 1001, accountHolderName = "Example Name", currentBalance = 5000 };

   //if accountNumber is null, return HTTP 404
   if (accountNumber == null)
    return NotFound("Account Number should be supplied");


   if (accountNumber == 1001)
    //if accountNumber is 1001, return the current balance value
    return Content(bankAccount.currentBalance.ToString());
   else
    //if accountNumber is not 1001, return HTTP 400
    return BadRequest("Account Number should be 1001");
  }
 }
}

