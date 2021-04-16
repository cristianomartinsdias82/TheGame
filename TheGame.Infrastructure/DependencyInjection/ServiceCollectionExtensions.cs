using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data.SqlClient;
using TheGame.Commands.SaveMatchData;
using TheGame.Common.Caching;
using TheGame.Common.SystemClock;
using TheGame.Data.Ef;
using TheGame.Infrastructure.Commands.SaveMatchData.Repository;
using TheGame.Infrastructure.Data.Caching;
using TheGame.Infrastructure.Queries.GetLeaderboards.Repository;
using TheGame.Infrastructure.SystemClock;
using TheGame.Queries.GetLeaderboards;

namespace TheGame.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TheGameDbContext>(options => options.UseSqlServer(configuration["TheGameSettings:DbConnectionString"]));
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();
            DbProviderFactories.RegisterFactory(configuration["TheGameSettings:DataProviderName"], SqlClientFactory.Instance);
            services.AddScoped<ITheGameCommandsRepository, TheGameCommandsRepository>();
            services.AddScoped<ITheGameQueriesRepository, TheGameQueriesRepository>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
