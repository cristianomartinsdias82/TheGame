using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheGame.Commands.SaveMatchData;
using TheGame.SharedKernel.Validation;

namespace TheGame.Commands.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDataInputValidation<SaveGameMatchDataRequest>, SaveGameMatchDataValidator>();

            return services;
        }
    }
}
