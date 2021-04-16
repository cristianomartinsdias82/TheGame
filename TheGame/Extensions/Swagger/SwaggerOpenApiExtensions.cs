using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;
using TheGame.Settings;
using TheGame.SharedKernel;

namespace TheGame.Extensions.Swagger
{
    public static class UseOpenApiExtensions
    {
        public static IServiceCollection AddSwaggerOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = new SwaggerSettings();
            configuration.Bind(nameof(SwaggerSettings), swaggerSettings);
            services.AddSingleton(swaggerSettings);
            var apiSettings = new TheGameSettings();
            configuration.Bind(nameof(TheGameSettings), apiSettings);

            services.AddSwaggerGen(options => {
                options.SwaggerDoc(apiSettings.CurrentVersion, new OpenApiInfo() { Title = apiSettings.Title, Version = apiSettings.CurrentVersion });
                options.ExampleFilters();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

            return services;
        }

        public static IApplicationBuilder UseSwaggerOpenApi(this IApplicationBuilder builder)
        {
            var apiSettings = builder.ApplicationServices.GetRequiredService<TheGameSettings>();
            var swaggerSettings = builder.ApplicationServices.GetRequiredService<SwaggerSettings>();

            builder.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerSettings.JsonRoute;
            });
            builder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    swaggerSettings.UiEndpoint.Replace("{CurrentVersion}", apiSettings.CurrentVersion),
                    swaggerSettings.Description);

                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            return builder;
        }
    }
}
