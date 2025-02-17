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

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Test Complete!");
app.MapGet("/test/{id}", (int id) => {return Results.Ok(id);});

app.MapPost("/test", (int test) => {return Results.Ok(test);});

app.Run(); 
