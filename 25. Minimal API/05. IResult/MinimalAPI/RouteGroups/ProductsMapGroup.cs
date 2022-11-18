
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

    return Results.Ok(products);
   });


   //GET /products/{id}
   group.MapGet("/{id:int}", async (HttpContext context, int id) =>
   {
    Product? product = products.FirstOrDefault(temp => temp.Id == id);
    if (product == null)
    {
     return Results.BadRequest(new { error = "Incorrect Product ID" });
    }

    //Sample Response:
    //{ "Id": 1, "ProductName": "Smart Phone" }

    return Results.Ok(product);
   });


   //POST /products
   group.MapPost("/", (HttpContext context, Product product) =>
   {
    products.Add(product);

    //Sample Response:
    //{ "message": "Product Added" }

    return Results.Ok(new { message = "Product Added" });
   });


   //PUT /products/{id}
   group.MapPut("/{id}", async (HttpContext context, int id, [FromBody]Product product) => {
    Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
    if (productFromCollection == null)
    {
     //Sample Response:
     //{ "error": "Invalid Product ID" }

     return Results.BadRequest(new { error = "Incorrect Product ID" });
    }

    productFromCollection.ProductName = product.ProductName;

    //Sample Response:
    //{ "message": "Product Updated" }

    return Results.Ok(new { message = "Product Updated" });
   });


   //DELETE /products/{id}
   group.MapDelete("/{id}", async (HttpContext context, int id) => {
    Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
    if (productFromCollection == null)
    {
     /* 
     Sample Response:
     {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "errors": {
          "id": [
              "Incorrect Product ID"
          ]
       }
     }
     */

     return Results.ValidationProblem(
      new Dictionary<string, string[]> {
       {  "id", new  string[] { "Incorrect Product ID" } }
      });
    }

    products.Remove(productFromCollection);

    //Sample Response:
    //{ "message": "Product Deleted" }

    return Results.Ok(new { message = "Product Deleted" });
   });


   return group;
  }
 }
}
