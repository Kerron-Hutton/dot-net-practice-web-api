using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service Dependency Injection
builder.Services.AddScoped<ISuperHeroService, SuperHeroService>();

// Entity Framework Configuration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(
        "Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=DotNetSampleAppOne;"
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Migrate latest database changes during startup
using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

// Here is the migration executed
dbContext.Database.Migrate();

app.Run();

public partial class Program
{
}