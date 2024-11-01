using Microsoft.EntityFrameworkCore;
using RocketFinApi.ActionFilters;
using RocketFinApi.Controllers;
using RocketFinApi.Core.Helpers;
using RocketFinApi.Extensions;
using RocketFinApp.Services;
using RocketFinApp.Services.Interfaces;
using RocketFinInfrastructure;
using RocketFinInfrastructure.Providers;
using RocketFinInfrastructure.Providers.Interfaces;
using RocketFinInfrastructure.Repositories;
using RocketFinInfrastructure.Repositories.Interfaces;
using RocketFinInfrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IMarketDataProvider, MarketDataProvider>();

builder.Services.AddHttpClient<MarketDataProvider>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseSqlServer(configuration.GetValue<string>("DefaultConnection")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingActionFilter>(); 
});

builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection String: {connectionString}"); // Should not be null
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(origins: configuration.GetValue<string>("AngularAppUrl")));

app.UseAuthorization();

app.MapControllers();

app.Run();
