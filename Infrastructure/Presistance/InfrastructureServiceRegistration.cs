
using Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistance.Data;
using Presistance.Reposatories;
using StackExchange.Redis;


namespace Presistance;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
           IConfiguration configration)
    {

        services.AddDbContext<StoreDbContetx>(options =>
        {
            var connectionString = configration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IDbinitializer, Dbinitializer>();
        services.AddScoped<IUnitOfWork, UnitofWork>();
        services.AddScoped<IBasketReposatory, BasketReposatory>();
        services.AddSingleton<IConnectionMultiplexer>((_) =>
        {
            return ConnectionMultiplexer.Connect(configration.GetConnectionString("RediusConnection")!);
        });
        return services;
    }
}