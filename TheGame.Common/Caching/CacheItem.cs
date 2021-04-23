using System;

namespace TheGame.Common.Caching
{
    public struct CacheItem<T>
    {
        public readonly Guid Id;
        public T Item { get; private set; }

        private CacheItem(T item)
        {
            Id = Guid.NewGuid();
            Item = item;
        }

        public static CacheItem<T> Create(T item)
        => new CacheItem<T>(item);
    }
}