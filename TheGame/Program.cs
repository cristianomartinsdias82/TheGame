using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheGame.MatchDataFlushingWorker;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using TheGame.Common.Caching;
//using TheGame.Queries.GetLeaderboards.Repositories;
//using TheGame.SharedKernel;
//using TheGame.Common.SystemClock;
//using TheGame.Commands.Repositories;

namespace TheGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services => {
                    services.AddHostedService<GameMatchesDataDbFlushingService>();
                    //(
                    //    provider => new GameMatchesDataDbFlushingService(
                    //        provider.GetService<IConfiguration>(),
                    //        provider.GetService<ILogger<GameMatchesDataDbFlushingService>>(),
                    //        provider.GetService<ICacheProvider>(),
                    //        provider.GetRequiredService<ITheGameCommandsRepository>(),
                    //        provider.GetRequiredService<ITheGameQueriesRepository>(),
                    //        provider.GetService<TheGameSettings>(),
                    //        provider.GetService<IDateTimeProvider>())
                    //);
                });
    }
}
