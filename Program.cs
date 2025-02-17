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
app.MapGet("/users", () => {
    db.Users.ToList();
});
app.MapGet("/users/{id}", (int id) => {
    var user = db.Users.FirstOrDefault(u => u.Id == id);
    return user;
});

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

//Orders
app.MapGet("/products", () => {  
    db.Users.ToList();
});
app.MapGet("/products/{id}", (int id) => { 
    var product = db.Products.FirstOrDefault(p => p.Id == id);
});

app.MapPost("/products", (Product product) => {
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

//Orders
app.MapGet("/orders", () => {  
    db.Orders.ToList();
});
app.MapGet("/orders/{id}", (int id) => { 
    var product = db.Orders.FirstOrDefault(p => p.Id == id);
});

app.MapPost("/orders", (Order order) => {
    if(db.Orders.Any(o => o.Id == order.Id)) 
    {
        return Results.BadRequest("The order id is already taken.");
    }
    db.Orders.Add(order);
    return Results.Created($"/users/{order.Id}", order);
    });

app.MapPut("/orders/{id}", (int id, Order updatedOrder) => {
    var order = db.Orders.FirstOrDefault(o => o.Id == id);
    if (order is null) return Results.NotFound("The order was not found");
    order.Id = updatedOrder.Id;
    return Results.Ok();
    });

app.MapDelete("/orders/{id}", (int id) => {
    var order = db.Orders.FirstOrDefault(o => o.Id == id);

    db.Orders.Remove(order);
    return Results.Ok(id);
    });

//Category
app.MapGet("/categories", () => {  
    db.Categories.ToList();
});
app.MapGet("/categories/{id}", (int id) => { 
    var category = db.Categories.FirstOrDefault(c => c.Id == id);
});

app.MapPost("/categories", (Category category) => {
    if(db.Categories.Any(c => c.Id == category.Id)) 
    {
        return Results.BadRequest("The order id is already taken.");
    }
    db.Categories.Add(category);
    return Results.Created($"/users/{category.Id}", category);
    });

app.MapPut("/category/{id}", (int id, Category updatedCategory) => {
    var category = db.Categories.FirstOrDefault(c => c.Id == id);
    if (category is null) return Results.NotFound("The order was not found");
    category.Id = updatedCategory.Id;
    return Results.Ok();
    });

app.MapDelete("/category/{id}", (int id) => {
    var category = db.Categories.FirstOrDefault(c => c.Id == id);

    db.Categories.Remove(category);
    return Results.Ok(id);
    });

app.Run(); 
