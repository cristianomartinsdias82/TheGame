using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheGame.Common.Caching
{
    public interface ICacheProvider
    {
        Task SetAsync<T>(T item, string key, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
    }
}
