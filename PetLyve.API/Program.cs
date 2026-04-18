using Microsoft.EntityFrameworkCore;
using PetLyve.Application;
using PetLyve.Infrastructure.Data;
using PetLyve.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

app.MapGet("/api/health", async (ApplicationDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? Results.Ok("Banco SQLite rodando lisinho! 🐾") : Results.StatusCode(500);
});

app.Run();