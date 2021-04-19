using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheGame.MatchDataFlushingWorker;
using TheGame.Infrastructure.Data.Ef.Migrations;
using TheGame.Infrastructure.Data.Caching;

namespace TheGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase()
                .PerformInitialCacheLoading()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<GameMatchesDataDbFlushingService>();
                });
    }
}

//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using TheGame.MatchDataFlushingWorker;
//using TheGame.Infrastructure.Data.Ef.Migrations;
//using TheGame.Infrastructure.Data.Caching;

//namespace TheGame
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args)
//                .Build()
//                .MigrateDatabase()
//                .PerformInitialCacheLoading()
//                .Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args)
//        {
//            var host = Host.CreateDefaultBuilder(args)
//                .ConfigureAppConfiguration(x => x.AddCommandLine(args))
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                })
//                .ConfigureServices(services =>
//                {
//                    services.AddHostedService<GameMatchesDataDbFlushingService>();
//                });

//            return host;
//        }
//    }
//}