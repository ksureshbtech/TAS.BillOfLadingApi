using BillOfLadingAPI.Repository;
using BillOfLadingAPI.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFillingTransactionService,FillingTransactionService>();
builder.Services.AddScoped<IFillingTransactionRepository,FillingTransactionRepository>();




// Add Serilog logging
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddCors();
var app = builder.Build();

// Configure CORS
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());



// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


app.MapGet("/api/report", async (long orderId, IFillingTransactionService service) =>
{
    var result = await service.GetBillOfLandingAsync(orderId);
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

app.MapGet("/api/reportbydate", async (DateTime? startDate, DateTime? endDate, IFillingTransactionService service) =>
{
    // Set default values if the parameters are not provided
    var start = startDate ?? DateTime.MinValue;
    var end = endDate ?? DateTime.MaxValue;

    var result = await service.GetBillOfLandingsAsync(start, end);
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}