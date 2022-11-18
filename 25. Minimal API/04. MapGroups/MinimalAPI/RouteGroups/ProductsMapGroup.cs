
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;
using System.Text.Json;

namespace MinimalAPI.RouteGroups
{
 public static class ProductsMapGroup
 {
  private static List<Product> products = new List<Product>() {
   new Product() { Id = 1, ProductName = "Smart Phone" },
   new Product() { Id = 2, ProductName = "Smart TV" }
  };

  public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
  {

   //GET /products
   group.MapGet("/", async (HttpContext context) =>
   {
    //Sample Response:
    //[{ "Id": 1, "ProductName": "Smart Phone" }, { "Id": 2, "ProductName": "Smart TV" }]

    await context.Response.WriteAsync(JsonSerializer.Serialize(products));
   });


   //GET /products/{id}
   group.MapGet("/{id:int}", async (HttpContext context, int id) =>
   {
    Product? product = products.FirstOrDefault(temp => temp.Id == id);
    if (product == null)
    {
     context.Response.StatusCode = 400; //Bad Request
     await context.Response.WriteAsync("Incorrect Product ID");
     return;
    }

    //Sample Response:
    //{ "Id": 1, "ProductName": "Smart Phone" }

    await context.Response.WriteAsync(JsonSerializer.Serialize(product));
   });


   //POST /products
   group.MapPost("/", async (HttpContext context, Product product) =>
   {
    products.Add(product);
    await context.Response.WriteAsync("Product Added");
   });


   //PUT /products/{id}
   group.MapPut("/{id}", async (HttpContext context, int id, [FromBody]Product product) => {
    Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
    if (productFromCollection == null)
    {
     context.Response.StatusCode = 400; //Bad Request
     await context.Response.WriteAsync("Incorrect Product ID");
     return;
    }

    productFromCollection.ProductName = product.ProductName;

    await context.Response.WriteAsync("Product Updated");
   });


   //DELETE /products/{id}
   group.MapDelete("/{id}", async (HttpContext context, int id) => {
    Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
    if (productFromCollection == null)
    {
     context.Response.StatusCode = 400; //Bad Request
     await context.Response.WriteAsync("Incorrect Product ID");
     return;
    }

    products.Remove(productFromCollection);

    await context.Response.WriteAsync("Product Deleted");
   });
   return group;
  }
 }
}
