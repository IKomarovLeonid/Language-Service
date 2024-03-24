using Business.Src.Objects;
using MediatR;

namespace Business.Src.Commands
{
    public class DeleteAttemptHistoryCommand : IRequest<StateResult>
    {
        public ulong Id { get; private set; }

        public DeleteAttemptHistoryCommand(ulong id)
        {
            Id = id;
        }
    }
}
