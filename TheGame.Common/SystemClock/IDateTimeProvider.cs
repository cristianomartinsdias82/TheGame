using System;

namespace TheGame.Common.SystemClock
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset DateTimeOffset { get; set; }
        public DateTime DateTime { get; set; }
    }
}
