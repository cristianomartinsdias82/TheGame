using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Common.Behaviors
{
    public class DataInputLoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;

        public DataInputLoggingPreProcessor(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw ArgNullEx(nameof(logger));
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"({typeof(TRequest).Name}) Input data => {JsonSerializer.Serialize(request)}");

            await Task.CompletedTask;
        }
    }
}
