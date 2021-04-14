using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheGame.SharedKernel;

namespace TheGame.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new TheGameSettings();
            configuration.Bind(nameof(TheGameSettings), settings);
            services.AddSingleton(settings);

            return services;
        }
    }
}
