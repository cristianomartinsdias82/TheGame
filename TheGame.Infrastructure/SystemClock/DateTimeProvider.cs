using System;
using TheGame.Common.SystemClock;

namespace TheGame.Infrastructure.SystemClock
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset DateTimeOffset { get => DateTimeOffset.UtcNow; }
        public DateTime DateTime { get => DateTime.UtcNow; }
    }
}
