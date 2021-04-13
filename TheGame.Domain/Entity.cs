using System;

namespace TheGame.Domain
{
    public class Entity<TKey> where TKey : IComparable, IComparable<TKey>, IConvertible, IEquatable<TKey>, IFormattable
    {
        public TKey Id { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
    }
}