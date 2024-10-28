using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;
using Objects.Dto;

namespace Business.Src.Handlers
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
                UserId = command.UserId,
                WordsErrors = string.Empty
            };
            var entity = await _histories.AddAsync(dto);

            return StateResult.Success(entity.Id);
        }
    }
}
