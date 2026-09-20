using Microsoft.EntityFrameworkCore;
using PetLyve.API.Exceptions;
using PetLyve.Application;
using PetLyve.Infrastructure.Data;
using PetLyve.Infrastructure.Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "PetLyve API",
        Version = "v1",
        Description = "API REST para gerenciamento de pets, donos e serviços."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/health", async (ApplicationDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok("Banco SQLite rodando lisinho! 🐾")
        : Results.StatusCode(500);
});

app.MapControllers();

app.Run();