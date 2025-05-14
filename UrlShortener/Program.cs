using Microsoft.EntityFrameworkCore;
using UrlShortener.Configuracoes;
using UrlShortener.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.ConfigurarCors();

builder.Services.ConfigurarPersistencias(builder.Configuration);

builder.Services.ConfigurarServicos();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>(); // Exemplo, substitua pelo seu DbContext

        Console.WriteLine("Attempting to apply migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully or already up-to-date.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
