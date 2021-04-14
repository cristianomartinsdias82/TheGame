using MediatR;
using System;

namespace TheGame.Commands.SaveMatchData
{
    public class SaveMatchDataRequest : IRequest<SaveMatchDataResponse>
    {
        public long PlayerId { get; set; }
        public long MatchId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}