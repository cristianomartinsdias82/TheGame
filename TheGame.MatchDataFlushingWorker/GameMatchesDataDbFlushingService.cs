using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.MatchDataFlushingWorker.Utilities;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.MatchDataFlushingWorker
{
    public class GameMatchesDataDbFlushingService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GameMatchesDataDbFlushingService> _logger;
        private readonly ICacheProvider _cacheProvider;
        private readonly TheGameSettings _settings;
        private Timer _timer;
        private Random _random = new Random();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _ongoingTask;

        public GameMatchesDataDbFlushingService(
            IConfiguration configuration,
            ILogger<GameMatchesDataDbFlushingService> logger,
            ICacheProvider cacheProvider,
            TheGameSettings settings)
        {
            _configuration = configuration ?? throw ArgNullEx(nameof(configuration));
            _logger = logger ?? throw ArgNullEx(nameof(logger));
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            ScheduleJobExecution();

            _logger.LogInformation($"{DateTime.UtcNow:O} - {nameof(GameMatchesDataDbFlushingService)} started.");

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.UtcNow:O} - {nameof(GameMatchesDataDbFlushingService)} stopped.");

            _cancellationTokenSource.Cancel();

            await Task.WhenAny(_ongoingTask, Task.Delay(-1, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task ExecuteAsync(object state, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.UtcNow:O} - Flushing data to database...");

            var matchData = await _cacheProvider.GetMatchDataAsync(_settings, cancellationToken);
            //TODO: Bulk insert logic
            
            _logger.LogInformation($"{DateTime.UtcNow:O} - Operation successful!");

            ScheduleJobExecution();
        }

        private void ScheduleJobExecution()
        {
            _ongoingTask = new Task(async (state) => await ExecuteAsync(state, _cancellationTokenSource.Token), _cancellationTokenSource.Token);

            //_timer = new Timer(async (state) => { await ExecuteAsync(state, _cancellationTokenSource.Token); },
            _timer = new Timer((state) => _ongoingTask.Start(),
                               null,
                               //TimeSpan.FromSeconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(-1));
        }
    }
}