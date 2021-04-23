using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TheGame.Commands.SaveMatchData;
using TheGame.Common.Behaviors;
using TheGame.Common.DependencyInjection;
using TheGame.Extensions.Swagger;
using TheGame.Infrastructure.DependencyInjection;
using TheGame.Queries.GetLeaderboards;

namespace TheGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var queriesAssembly = typeof(GetLeaderboardsRequest).Assembly;
            var commandsAssembly = typeof(SaveGameMatchDataRequest).Assembly;
            var commonAssembly = typeof(DataInputLoggingPreProcessor<>).Assembly;

            services.AddControllers();
            services.AddSharedKernel(Configuration);
            services.AddSwaggerOpenApi(Configuration);
            services.AddMediatR(queriesAssembly, commandsAssembly, commonAssembly);
            services.AddBehaviors(Configuration);
            services.AddValidatorsFromAssemblies(new Assembly[] { queriesAssembly, commandsAssembly });
            services.AddInfrastructure(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerOpenApi();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
