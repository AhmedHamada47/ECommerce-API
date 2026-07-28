using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Services.Assembly).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }
    }
}
