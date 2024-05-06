using chairs_dotnet7_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);
chairs.MapGet("/{id}", GetChairById);
chairs.MapGet("/{name}", GetChairByName);
chairs.MapPost("/", addChair);
chairs.MapPut("/{id}", updateChair);
chairs.MapPut("/{id}/stock", updateOnlyStockChair);
chairs.MapPost("/chair/purchase", buyChair);
chairs.MapDelete("/{id}", deleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
async Task<IResult> GetChairs(DataContext db)
{
    return TypedResults.Ok();
}

async Task<IResult> addChair(Chair chair)
{
    return TypedResults.Ok();
}
