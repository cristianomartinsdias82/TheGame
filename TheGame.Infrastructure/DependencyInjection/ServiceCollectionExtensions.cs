using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheGame.Common.Caching;
using TheGame.Data.Ef;
using TheGame.Infrastructure.Data.Caching;

namespace TheGame.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TheGameDbContext>(options => options.UseSqlServer(configuration["TheGameSettings:DbConnectionString"]));
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();

            return services;
        }
    }
}
