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
chairs.MapPost("/purchase", buyChair);
chairs.MapPut("/{id}", updateChair);
chairs.MapPut("/{id}/stock", updateOnlyStockChair);
chairs.MapDelete("/{id}", deleteChair);
app.Run();

//TODO: ENDPOINTS SOLICITADOS
//endpointed
async Task<IResult> GetChairs(DataContext db)
{
    List<Chair> chairlist = await db.Chairs.ToListAsync();
    return TypedResults.Ok(chairlist);
}

async Task<Chair?> GetChairById(int id, DataContext db)
{
    Chair? foundChair = await db.Chairs.FindAsync(id);
    return foundChair;
}
//endpointed
async Task<IResult> GetChairByName(string nombre, DataContext db)
{
    Chair? foundChair = await db.Chairs.Where(u => u.Nombre == nombre).FirstOrDefaultAsync();
    if(foundChair is null) return TypedResults.NotFound("no se encontró la silla con ese nombre");
    return TypedResults.Ok(foundChair);
}
//endpointed
async Task<IResult> addChair([FromBody] Chair chair, DataContext db)
{
    Chair? foundChair = await db.Chairs.Where(u => u.Nombre == chair.Nombre).FirstOrDefaultAsync();
    if(foundChair is not null) return TypedResults.Ok("Esa silla ya está en el sistema");
    await db.Chairs.AddAsync(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created("Silla Creada");
}

async Task<IResult> buyChair([FromBody] BuyChairDTO buyChairDto, DataContext db)
{
    Chair? foundChair = await GetChairById(buyChairDto.Id, db);
    if(foundChair is null) return TypedResults.BadRequest("no se encontró la silla con el id");
    if(buyChairDto.Stock > foundChair.Stock) return TypedResults.BadRequest("El stock seleccionado es mayor que el que hay");
    int totalcosto = foundChair.Precio * buyChairDto.Stock;
    if(buyChairDto.TotalPagado < totalcosto) return TypedResults.BadRequest("Usted debe pagar el precio que corresponde que son: " + totalcosto);
    foundChair.Stock = foundChair.Stock - buyChairDto.Stock;
    await db.SaveChangesAsync();
    return TypedResults.Ok("Compra realizada");
}


async Task<IResult> updateChair(int id, [FromBody] UpdateChairDTO updateChairDTO, DataContext db)
{
    Chair? foundChair = await GetChairById(id, db);
    if(foundChair is null) return TypedResults.NotFound("no se encontró la silla con el id");
    foundChair.Nombre = updateChairDTO.Nombre;
    foundChair.Tipo = updateChairDTO.Tipo;
    foundChair.Material = updateChairDTO.Material;
    foundChair.Color = updateChairDTO.Color;
    foundChair.Altura = updateChairDTO.Altura;
    foundChair.Anchura = updateChairDTO.Anchura;
    foundChair.Profundidad = updateChairDTO.Profundidad;
    foundChair.Precio = updateChairDTO.Precio;
    await db.SaveChangesAsync();
    return TypedResults.NoContent();

}

async Task<IResult> updateOnlyStockChair(int id, [FromBody] UpdateStockDTO updateStockDTO, DataContext db)
{
    Chair? foundChair = await GetChairById(id, db);
    if(foundChair is null) return TypedResults.NotFound("no se encontró la silla con el id");
    foundChair.Stock = foundChair.Stock + updateStockDTO.Stock;
    await db.SaveChangesAsync();
    return TypedResults.Ok(foundChair);

}

async Task<IResult> deleteChair(int id, DataContext db)
{
    Chair? foundChair = await GetChairById(id, db);
    if(foundChair is null) return TypedResults.NotFound("no se encontró la silla con el id");
    db.Chairs.Remove(foundChair);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}


