using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel.Validation;

namespace TheGame.Queries.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDataInputValidation<GetLeaderboardsRequest>, GetLeaderboardsValidator>();

            return services;
        }
    }
}
