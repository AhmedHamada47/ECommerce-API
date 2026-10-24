using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ECommerce.Tests
{
    internal static class TestMapper
    {
        public static IMapper Create()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["BaseUrl"] = "http://localhost:5000"
                })
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)
                .AddTransient<Services.MappingProfiles.PictureUrlResolver>()
                .AddAutoMapper(cfg => cfg.AddMaps(typeof(Services.Assembly).Assembly))
                .BuildServiceProvider();

            return serviceProvider.GetRequiredService<IMapper>();
        }
    }
}
