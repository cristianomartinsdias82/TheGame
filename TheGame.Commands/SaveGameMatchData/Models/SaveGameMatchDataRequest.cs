using MediatR;
using System;

namespace TheGame.Commands.SaveMatchData
{
    public class SaveGameMatchDataRequest : IRequest<SaveGameMatchDataResponse>
    {
        public long GameId { get; set; }
        public long PlayerId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}