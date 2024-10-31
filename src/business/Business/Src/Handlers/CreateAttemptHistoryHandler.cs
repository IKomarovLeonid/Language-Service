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
        private readonly IRepository<AttemptHistoryDto> _histories;

        public CreateAttemptHistoryHandler(IRepository<AttemptHistoryDto> histories)
        {
            _histories = histories;
        }

        public async Task<StateResult> Handle(CreateAttemptHistoryCommand command, CancellationToken cancellationToken)
        {
            var dto = new AttemptHistoryDto()
            {
                TotalAttempts = command.TotalAttempts,
                CorrectAttempts = command.CorrectAttempts,
                WordsErrors = command.WordErrors,
                AttemptAttributes = command.AttemptAttributes
            };
            var entity = await _histories.AddAsync(dto);

            return StateResult.Success(entity.Id);
        }
    }
}
