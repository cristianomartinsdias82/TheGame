using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheGame.Commands.DependencyInjection;
using TheGame.Commands.SaveMatchData;
using TheGame.Common.DependencyInjection;
using TheGame.Extensions.Swagger;
using TheGame.Infrastructure.DependencyInjection;
using TheGame.Queries.DependencyInjection;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSharedKernel(Configuration);
            services.AddSwaggerOpenApi(Configuration);
            services.AddMediatR(typeof(GetLeaderboardsRequest).Assembly, typeof(SaveGameMatchDataRequest).Assembly);
            services.AddQueries(Configuration);
            services.AddCommands(Configuration);
            services.AddInfrastructure(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
