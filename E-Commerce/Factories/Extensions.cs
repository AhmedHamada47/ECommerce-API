using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Factories
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           
            return services;
        }
        public static IServiceCollection AddWebApplicationServices(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Func<ActionContext, IActionResult>  
                options.InvalidModelStateResponseFactory = APIResponseFactory.GenerateAPIValidationResponse;
            });
            services.AddSwaggerServices();
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
