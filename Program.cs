using Api_project;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Minimal API";
    config.Title = "MinimalAPI v1";
    config.Version = "v1";
});
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
app.MapGet("/users", async (AppDbContext db) => 
{
    var users = await db.Users.ToListAsync();
    return Results.Ok(users);
});

app.MapGet("/users/{id}", async (int id, AppDbContext db) => 
{
    var user = await db.Users.FindAsync(id);
    
    return user is not null
    ? Results.Ok(user)
    : Results.NotFound($"user-id {id} not found");
    
});

app.MapPost("/users", async (User user, AppDbContext db) => 
{
    if (await db.Users.AnyAsync(u => u.Id == user.Id))
    {
        return Results.BadRequest("The user-id is already taken.");
    }
    
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
    });

app.MapPut("/users/{id}", async (int id, User updatedUser, AppDbContext db) => 
{
    if (updatedUser is null || string.IsNullOrWhiteSpace(updatedUser.Name))
    {
        return Results.BadRequest("Invalid user data.");
    }

    var user = await db.Users.FindAsync(id);
    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    user.Name = updatedUser.Name;

    await db.SaveChangesAsync(); 

    return Results.Ok(user); 
});

app.MapDelete("/users/{id}", async (int id, AppDbContext db) => 
{
    var user = await db.Users.FindAsync(id);
    if(user is null)
    {
        return Results.NotFound($"User-id {id} not found");
    }

    db.Users.Remove(user);
    await db.SaveChangesAsync();

    return Results.Ok(id);
});

//Products
app.MapGet("/products", async (AppDbContext db) => 
{  
    return await db.Products.ToListAsync();
});

app.MapGet("/products/{id}", async (int id, AppDbContext db) => 
{
    var product = await db.Products.FindAsync(id);
    return product is not null
    ? Results.Ok(product)
    : Results.NotFound($"Product with ID {id} not found.");
});

app.MapPost("/products", async (Product product, AppDbContext db) => 
{
    if( await db.Products.AnyAsync(p => p.Id == product.Id)) 
    {
        return Results.BadRequest("The product id is already taken.");
    }
    
    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{product.Id}", product);
});

app.MapPut("/product/{id}", async (int id, Product updatedProduct, AppDbContext db) => 
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound("The product was not found");
    }
    
    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.CategoryId = updatedProduct.CategoryId;

    await db.SaveChangesAsync();
    
    return Results.Ok(product);
});

app.MapDelete("/product/{id}", async (int id, AppDbContext db) => 
{
    var product = await db.Products.FindAsync(id);
    if(product is null)
    {
        return Results.NotFound($"Product-id {id} not found");
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.Ok(id);
    });

//Orders
app.MapGet("/orders", async (AppDbContext db) => 
{  
    return await db.Orders.ToListAsync();
});

app.MapGet("/orders/{id}", async (int id, AppDbContext db) => 
{ 
    var product = await db.Orders.FindAsync(id);

    return product is not null
    ? Results.Ok(product)
    : Results.NotFound($"Order-id {id} not found");
});

app.MapPost("/orders", async (Order order, AppDbContext db) => 
{
    if(await db.Orders.AnyAsync(o => o.Id == order.Id)) 
    {
        return Results.BadRequest("The order id is already taken.");
    }

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{order.Id}", order);
});

app.MapPut("/orders/{id}", async (int id, Order updatedOrder, AppDbContext db) => 
{
    if (updatedOrder is null || updatedOrder.Products is null || !updatedOrder.Products.Any())
    {
        return Results.BadRequest("Invalid order data.");
    }

    var order = await db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);
    if (order is null)
    {
        return Results.NotFound("The order was not found.");
    }

    order.UserId = updatedOrder.UserId;

    order?.Products?.Clear();
    order?.Products?.AddRange(updatedOrder.Products);

    await db.SaveChangesAsync(); 

    return Results.Ok(order);
});

app.MapDelete("/orders/{id}", async (int id, AppDbContext db) => 
{
    var order = await db.Orders.FindAsync(id);
    if (order is null)
    {
        return Results.NotFound($"Order id {id} not found");
    }

    db.Orders.Remove(order);
    await db.SaveChangesAsync();

    return Results.Ok(id);
});

//Category
app.MapGet("/categories", async (AppDbContext db) => 
{  
    return await db.Categories.ToListAsync();
});

app.MapGet("/categories/{id}", async (int id, AppDbContext db) => 
{ 
    var category = await db.Categories.FindAsync(id);

    return category is not null
    ? Results.Ok(category)
    : Results.NotFound($"Category id {id} not found");
});

app.MapPost("/categories", async (Category category, AppDbContext db) => 
{
    if(await db.Categories.AnyAsync(c => c.Id == category.Id)) 
    {
        return Results.BadRequest("The category id is already taken.");
    }

    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{category.Id}", category);
});

app.MapPut("/category/{id}", async (int id, Category updatedCategory, AppDbContext db) => 
{
    if (updatedCategory is null || string.IsNullOrWhiteSpace(updatedCategory.Name))
    {
        return Results.BadRequest("Invalid category data.");
    }

    var category = await db.Categories.FindAsync(id);
    if (category is null)
    {
        return Results.NotFound($"Category id {id} not found.");
    }

    category.Name = updatedCategory.Name;
    await db.SaveChangesAsync();

    return Results.Ok(category); 
});

app.MapDelete("/category/{id}", async (int id, AppDbContext db) => 
{
    var category = await db.Categories.FindAsync(id);

    if (category is null)
    {
        return Results.NotFound($"Category id {id} not found");
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.Ok(id);
});

app.Run(); 
