using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;

namespace Business.Commands
{
    public class GetGamesHistoryCommand : IRequest<SelectResult<GameAttemptModel>>
    {
        public ulong? UserId { get; set; }
    }
}
