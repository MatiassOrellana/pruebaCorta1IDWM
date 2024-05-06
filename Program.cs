using chairs_dotnet7_api;
using Microsoft.AspNetCore.Http.HttpResults;
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
chairs.MapGet("/{nombre}", GetChairByName);
chairs.MapPost("/", addChair);
chairs.MapPost("/chair/purchase", buyChair);
chairs.MapPut("/{id}", updateChair);
chairs.MapPut("/{id}/stock", updateOnlyStockChair);
chairs.MapDelete("/{id}", deleteChair);
app.Run();

//TODO: ENDPOINTS SOLICITADOS
async Task<IResult> GetChairs(DataContext db)
{
    List<Chair> chairlist = await db.Chairs.ToListAsync();
    return TypedResults.Ok(chairlist);
}

async Task<Chair> GetChairById(int id, DataContext db)
{
    Chair? foundChair = await db.Chairs.FindAsync(id);
    return foundChair;
}

async Task<IResult> GetChairByName(string nombre, DataContext db)
{
    Chair? foundChair = await db.Chairs.Where(u => u.Nombre == nombre).FirstOrDefaultAsync();
    if(foundChair is null) return TypedResults.NotFound("no se encontró la silla con ese nombre");
    return TypedResults.Ok(foundChair);
}

async Task<IResult> addChair(Chair chair)
{
    return TypedResults.Ok();
}

async Task buyChair(HttpContext context)
{
    throw new NotImplementedException();
}

async Task updateChair(HttpContext context)
{
    throw new NotImplementedException();
}

async Task updateOnlyStockChair(HttpContext context)
{
    throw new NotImplementedException();
}

async Task deleteChair(HttpContext context)
{
    throw new NotImplementedException();
}


