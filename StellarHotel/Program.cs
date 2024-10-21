using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StellarHotel.Context;
using StellarHotel.Data;
using StellarHotel.Helpers;
using StellarHotel.Interfaces;
using StellarHotel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Reservations Stellar Stay Hotel API",
        Description = "API for managing hotel room reservations and dynamic pricing",
        Contact = new OpenApiContact
        {
            Name = "Leonardo Fabián Hernández Peña",
            Email = "ingenieroleonardo@outlook.com",
            Url = new Uri("https://github.com/leo7962")
        },
        License = new OpenApiLicense
        {
            Name = "GPL-3.0"
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPricingService, PricingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedRooms(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during seeding the database");
}

app.Run();