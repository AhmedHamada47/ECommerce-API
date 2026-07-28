using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Factories
{
    public static class Extensions
    {
        public static IServiceCollection AddWebApplicationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = APIResponseFactory.GenerateAPIValidationResponse;
            });
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbinitializer>();
            await dbInitializer.InitilizeAsync();
            return app;
        }
    }
}