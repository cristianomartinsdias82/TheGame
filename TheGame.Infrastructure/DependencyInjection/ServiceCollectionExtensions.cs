using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheGame.Common.Caching;
using TheGame.Common.SystemClock;
using TheGame.Data.Ef;
using TheGame.Infrastructure.Data.Caching;
using TheGame.Infrastructure.SystemClock;

namespace TheGame.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TheGameDbContext>(options => options.UseSqlServer(configuration["TheGameSettings:DbConnectionString"]));
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
