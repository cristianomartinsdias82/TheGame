using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TheGame.Common.Caching;

namespace TheGame.Common.Dto
{
    public class CachedMatchGameDataComparer : IEqualityComparer<CacheItem<GameMatchDataDto>>
    {
        public bool Equals([AllowNull] CacheItem<GameMatchDataDto> x, [AllowNull] CacheItem<GameMatchDataDto> y)
        => x.Id == y.Id;

        public int GetHashCode([DisallowNull] CacheItem<GameMatchDataDto> obj)
        => (obj.Id.GetHashCode() + obj.Item.GameId.GetHashCode() * obj.Item.PlayerId.GetHashCode()) * 17;
    }
}
