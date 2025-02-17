var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
config.DocumentName = "Minimal API";
config.Title = "MinimalAPI v1";
config.Version = "v1";
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
config.DocumentTitle = "MinimalAPI";
config.Path = "/swagger";
config.DocumentPath =
"/swagger/{documentName}/swagger.json";
config.DocExpansion = "list";
});
}
// Users
app.MapGet("/users", () => "Hello World!");
app.MapGet("/users/{id}", (int id) => "Hello World!");

app.MapPost("/users", (User user) => {
    if(users.Any(u => u.Id == user.Id)) 
    {
        return Results.BadRequest("The user-id is already taken.");
    }
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
    });

app.MapPut("/users/{id}", (int id, User updatedUser) => {
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound("The user was not found");
    user.Name = updatedUser.Name;
    return Results.Ok();
    });

app.MapDelete("/users/{id}", (int id) => {
     var user = users.FirstOrDefault(u => u.Id == id);

   if(users.Any(u => u.Id == user.Id)) 
    {
        return Results.BadRequest("The user-id is already taken.");
    }
    users.Remove(id);
    return Results.Ok(id);
    });

//Product
app.MapGet("/proucts", () => "Hello World!");
app.MapGet("/proucts/{id}", (int id) => "Hello World!");

app.MapPost("/proucts", (Product product) => {
    if(products.Any(p => p.Id == product.Id)) 
    {
        return Results.BadRequest("The product id is already taken.");
    }
    products.Add(product);
    return Results.Created($"/users/{product.Id}", product);
    });

app.MapPut("/product/{id}", (int id, Products updatedProduct) => {
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound("The user was not found");
    product.Name = updatedProduct.Name;
    return Results.Ok();
    });

app.MapDelete("/product/{id}", (int id) => {
     var product = products.FirstOrDefault(u => u.Id == id);

   if(products.Any(p => p.Id == p.Id)) 
    {
        return Results.BadRequest("The product id is already taken.");
    }
    products.Remove(id);
    return Results.Ok(id);
    });


app.Run(); 
