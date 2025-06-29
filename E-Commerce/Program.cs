using Domain.Contracts;
using E_Commerce.Factories;
using E_Commerce.MiddleWarers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistance;
using Presistance.Data;
using Presistance.Reposatories;
using Services;
using ServicesAbstractions;
using Shared.ErrorModels;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddWebApplicationServices();

        var app = builder.Build();
        await app.InitializeDatabaseAsync();
        // Initialize database
        app.UseMiddleware<CustomExHandlerMiddleware>();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

// Db Initialization Extension
//public static class DbInitializerExtension
//{
//    public static async Task InitializeDbAsync(this WebApplication app)
//    {
//        using var scope = app.Services.CreateScope();
//        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbinitializer>();
//        await dbInitializer.InitilizeAsync();
//    }
//}
