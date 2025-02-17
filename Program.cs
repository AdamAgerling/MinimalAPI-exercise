using Api_project;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Minimal API";
    config.Title = "MinimalAPI v1";
    config.Version = "v1";
});

var context = new AppDbContext();
using var db = new AppDbContext();
context.Database.EnsureCreated();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "MinimalAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// Users
app.MapGet("/users", () => "Hello World!");
app.MapGet("/users/{id}", (int id) => "Hello World!");

app.MapPost("/users", (User user) => {
    if (db.Users.Any(u => u.Id == user.Id))
    {
        return Results.BadRequest("The user-id is already taken.");
    }
    db.Users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
    });

app.MapPut("/users/{id}", (int id, User updatedUser) => {
    var user = db.Users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound("The user was not found");
    user.Name = updatedUser.Name;
    return Results.Ok();
    });

app.MapDelete("/users/{id}", (int id) => {
    var user = db.Users.FirstOrDefault(u => u.Id == id);

    db.Users.Remove(user);
    return Results.Ok(id);
    });

//Product
app.MapGet("/proucts", () => "Hello World!");
app.MapGet("/proucts/{id}", (int id) => "Hello World!");

app.MapPost("/proucts", (Product product) => {
    if(db.Products.Any(p => p.Id == product.Id)) 
    {
        return Results.BadRequest("The product id is already taken.");
    }
    db.Products.Add(product);
    return Results.Created($"/users/{product.Id}", product);
    });

app.MapPut("/product/{id}", (int id, Product updatedProduct) => {
    var product = db.Products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound("The user was not found");
    product.Name = updatedProduct.Name;
    return Results.Ok();
    });

app.MapDelete("/product/{id}", (int id) => {
    var product = db.Products.FirstOrDefault(u => u.Id == id);

    db.Products.Remove(product);
    return Results.Ok(id);
    });


app.Run(); 
