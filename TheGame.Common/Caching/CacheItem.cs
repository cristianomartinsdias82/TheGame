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

    //public class CacheItem<T>
    //{
    //    public Guid Id { get; set; } = Guid.NewGuid();

    //    public T Item { get; private set; }

    //    private CacheItem(T item)
    //    {
    //        Item = item;
    //    }
    //    public static CacheItem<T> Create(T item)
    //        => new CacheItem<T>(item);
    //}
}