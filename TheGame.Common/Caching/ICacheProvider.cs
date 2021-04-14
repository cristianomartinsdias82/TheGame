using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheGame.Common.Caching
{
    public interface ICacheProvider
    {
        Task SetAsync<T>(T data, string key, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
        Task ClearAsync(string key, CancellationToken cancellationToken);
    }
}
