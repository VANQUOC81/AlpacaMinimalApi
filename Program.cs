using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Services;

var builder = WebApplication.CreateBuilder(args);
// adds the database context to the dependency injection (DI)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
// enables displaying database-related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// only use AddEndpointsApiExplorer if you use v6's "minimal APIs":
// app.MapGet("/", () => "Hello World!");
builder.Services.AddEndpointsApiExplorer();
// add swagger generator
builder.Services.AddSwaggerGen(
options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.EnableAnnotations();
});

var app = builder.Build();

// Enable the middleware for serving the generated JSON document and the Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// organizes groups to reduce repetitive code and allows for customizing entire groups of endpoints with a single call
var todoItems = app.MapGroup("/todoitems");
var apiAlpaca = app.MapGroup("/api/alpaca/v1");

// MapGet and with annotations.
todoItems.MapGet("/", TodoService.GetAllTodos).WithMetadata(new SwaggerOperationAttribute("summary001", "description001")); ;
todoItems.MapGet("/complete", TodoService.GetCompleteTodos);
todoItems.MapGet("/{id}", TodoService.GetTodo);
todoItems.MapPost("/", TodoService.CreateTodo);
todoItems.MapPut("/{id}", TodoService.UpdateTodo);
todoItems.MapDelete("/{id}", TodoService.DeleteTodo);

apiAlpaca.MapGet("/getclock", AlpacaService.GetClock);
apiAlpaca.MapGet("/getaccount", AlpacaService.GetAccount);
apiAlpaca.MapGet("/listassets/{exchange}", AlpacaService.ListAssets);
apiAlpaca.MapPost("/order/submitmarketbuyorder", AlpacaService.SubmitMarketBuyOrder);
apiAlpaca.MapPost("/order/cancelorder/{orderid}", AlpacaService.CancelOrder);
apiAlpaca.MapPost("/order/submitmarketsellorder/{symbol}", AlpacaService.SubmitMarketSellOrder);

app.Run();
