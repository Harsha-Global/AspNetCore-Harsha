using MinimalAPI.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Product> products = new List<Product>() { 
 new Product() { Id = 1, ProductName = "Smart Phone" },
 new Product() { Id = 2, ProductName = "Smart TV" }
};

//GET /products
app.MapGet("/products", async (HttpContext context) => {
 //Sample Response:
 //[{ "Id": 1, "ProductName": "Smart Phone" }, { "Id": 2, "ProductName": "Smart TV" }]

 await context.Response.WriteAsync(JsonSerializer.Serialize(products));
});


//GET /products/{id}
app.MapGet("/products/{id:int}", async (HttpContext context, int id) =>
 {
  Product? product = products.FirstOrDefault(temp => temp.Id == id);
  if (product == null)
  {
   context.Response.StatusCode = 400; //Bad Request
   await context.Response.WriteAsync("Incorrect Product ID");
   return;
  }

  await context.Response.WriteAsync(JsonSerializer.Serialize(product));
});


//POST /products
app.MapPost("/products", async (HttpContext context, Product product) => {
 products.Add(product);
 await context.Response.WriteAsync("Product Added");
});


app.Run();
