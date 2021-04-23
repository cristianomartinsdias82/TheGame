using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Common.Behaviors
{
    public class PerformanceMeasurementBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceMeasurementBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();

            _logger = logger ?? throw ArgNullEx(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            var loggingData = $"@{request} took {elapsedMilliseconds}ms to run.";
            if (elapsedMilliseconds > 500)
                _logger.LogWarning($"Long running request! {loggingData}");
            else
                _logger.LogInformation(loggingData);

            return response;
        }
    }
}
