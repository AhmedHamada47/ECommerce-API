using Domain.Contracts;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Persistence;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
           IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
        .AddEntityFrameworkStores<StoreDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddSingleton<IConnectionMultiplexer>((_) =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
        });
        return services;
    }
}