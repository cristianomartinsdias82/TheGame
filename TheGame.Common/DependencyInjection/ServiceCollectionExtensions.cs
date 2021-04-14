using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheGame.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
