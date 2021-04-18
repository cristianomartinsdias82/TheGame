using Swashbuckle.AspNetCore.Filters;
using System;

namespace TheGame.Controllers.SaveMatchData
{
    public class SaveGameMatchDataExample : IExamplesProvider<SaveGameMatchDataDto>
    {
        public SaveGameMatchDataDto GetExamples()
        => new SaveGameMatchDataDto()
            {
                Timestamp = DateTimeOffset.UtcNow,
                GameId = 1,
                PlayerId = 1,
                Win = 326845
            };
    }
}
