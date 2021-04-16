using System;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.SharedKernel
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertUtcToLocalDateTime(this DateTimeOffset dateTime, TheGameSettings settings)
        {
            if (settings == null)
                throw ArgNullEx(nameof(settings));

            var timezone = TimeZoneInfo.FindSystemTimeZoneById(settings.Timezone);

            var offset = timezone.GetUtcOffset(dateTime);

            var localTime = dateTime.Add(offset).DateTime;

            return localTime;
        }
    }
}