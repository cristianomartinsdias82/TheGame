using System;

namespace TheGame.Common.SystemClock
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset DateTimeOffset { get; }
        public DateTime DateTime { get; }
    }
}
