var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Terminating middleware
app.Run(async (HttpContext context) =>
{
 if (context.Request.Method == "GET" && context.Request.Path == "/")
 {
  int firstNumber = 0, secondNumber = 0;
  string? operation = null;
  long? result = null;

  //read 'firstNumber' if submitted in the request body
  if (context.Request.Query.ContainsKey("firstNumber"))
  {
   string firstNumberString = context.Request.Query["firstNumber"][0];
   if (!string.IsNullOrEmpty(firstNumberString))
   {
    firstNumber = Convert.ToInt32(firstNumberString);
   }
   else
   {
    context.Response.StatusCode = 400;
    await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
   }
  }
  else
  {
   if (context.Response.StatusCode == 200)
    context.Response.StatusCode = 400;
   await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
  }

  //read 'secondNumber' if submitted in the request body
  if (context.Request.Query.ContainsKey("secondNumber"))
  {
   string secondNumberString = context.Request.Query["secondNumber"][0];
   if (!string.IsNullOrEmpty(secondNumberString))
   {
    secondNumber = Convert.ToInt32(context.Request.Query["secondNumber"][0]);
   }
   else
   {
    if (context.Response.StatusCode == 200)
     context.Response.StatusCode = 400;
    await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
   }
  }
  else
  {
   if (context.Response.StatusCode == 200)
    context.Response.StatusCode = 400;
   await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
  }

  //read 'operation' if submitted in the request body
  if (context.Request.Query.ContainsKey("operation"))
  {
   operation = Convert.ToString(context.Request.Query["operation"][0]);

   //perform the calculation based on the value of "operation"
   switch (operation)
   {
    case "add": result = firstNumber + secondNumber; break;
    case "subtract": result = firstNumber - secondNumber; break;
    case "multiply": result = firstNumber * secondNumber; break;
    case "divide": result = (secondNumber != 0) ? firstNumber / secondNumber : 0; break; //avoid DivideByZeroException, if secondNuber is 0 (zero)
    case "mod": result = (secondNumber != 0) ? firstNumber % secondNumber : 0; break; //avoid DivideByZeroException, if secondNuber is 0 (zero)
   }

   //If no case matched above, the "result" remains as 'null'
   if (result.HasValue)
   {
    await context.Response.WriteAsync(result.Value.ToString());
   }

   //if invalid value is submitted for "operation" parameter
   else
   {
    if (context.Response.StatusCode == 200)
     context.Response.StatusCode = 400;
    await context.Response.WriteAsync("Invalid input for 'operation'\n");
   }

  } //end of "of ContainsKey("operation")

  //if the "operation" parameter is submitted from the client
  else
  {
   if (context.Response.StatusCode == 200)
    context.Response.StatusCode = 400;
   await context.Response.WriteAsync("Invalid input for 'operation'\n");
  }
 }
});

app.Run();

