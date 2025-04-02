using System.Threading;
using System.Threading.Tasks;
using Business.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Dto;

namespace Business.Handlers
{
    internal class CreateAttemptHistoryHandler : IRequestHandler<CreateAttemptHistoryCommand, StateResult>
    {
        private readonly IRepository<GameAttemptDto> _histories;

        public CreateAttemptHistoryHandler(IRepository<GameAttemptDto> histories)
        {
            _histories = histories;
        }

        public async Task<StateResult> Handle(CreateAttemptHistoryCommand command, CancellationToken cancellationToken)
        {
            

            return StateResult.Success(1);
        }
    }
}
