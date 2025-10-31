using Microsoft.EntityFrameworkCore;
using PersonMinimalApi;
using PersonMinimalApi.DAL;
using PersonMinimalApi.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

try //Migrator
{
    using var scope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var apiV1 = app.MapGroup("/api/v1");

//app.UseHttpsRedirection();

apiV1.MapGet("/persons/{personId}", async (int personId, PersonService service) =>
    {
        var res = await service.GetById(personId);
        if (res != null) return Results.Ok(res);
        return Results.NotFound();
    })
    .WithName("GetPersonById")
    .WithOpenApi();

apiV1.MapGet("/persons", (PersonService service) => service.GetAll())
    .WithName("GetPersons")
    .WithOpenApi();

apiV1.MapPost("/persons", async (PersonRequest request, PersonService service) =>
    {
        var res = await service.Create(request);
        return Results.Created("/persons/{person.Id}", res);;
    })
    .WithName("CreatePerson")
    .WithOpenApi();

apiV1.MapPatch("/persons/{personId}", async (int personId, PersonPatchRequest patch, PersonService service) =>
    {
        var res = await service.Update(personId, patch);
        if  (res != null) return Results.Ok(res);
        return Results.NotFound();
    })
    .WithName("UpdatePerson")
    .WithOpenApi();

apiV1.MapDelete("/persons/{personId}", async (int personId, PersonService service) =>
    {
        var res = await service.Delete(personId);
        if  (res != null) return Results.NoContent();
        return Results.NotFound();
    })
    .WithName("DeletePerson")
    .WithOpenApi();

app.Run();


// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };
//
// app.MapGet("/weatherforecast", () =>
//     {
//         var forecast = Enumerable.Range(1, 5).Select(index =>
//                 new WeatherForecast
//                 (
//                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                     Random.Shared.Next(-20, 55),
//                     summaries[Random.Shared.Next(summaries.Length)]
//                 ))
//             .ToArray();
//         return forecast;
//     })
//     .WithName("GetWeatherForecast")
//     .WithOpenApi();



// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }