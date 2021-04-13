using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TheGame.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
