using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using Objects.Src.Models;
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
        private readonly IRepository<AttemptDto> _attempts;

        public CreateAttemptHistoryHandler(IRepository<AttemptHistoryDto> histories, IRepository<AttemptDto> attempts)
        {
            _histories = histories;
            _attempts = attempts;
        }

        public async Task<StateResult> Handle(CreateAttemptHistoryCommand command, CancellationToken cancellationToken)
        {
            var dto = new AttemptHistoryDto()
            {
                TotalAttempts = command.TotalAttempts,
                TotalSeconds = command.TotalSeconds,
                CorrectAttempts = command.CorrectAttempts,
                UserId = command.UserId,
                WordTypes = command.WordTypes.HasValue? command.WordTypes.Value : WordType.Any,
                Category = command.Category.HasValue ? command.Category.Value : WordCategory.Any
            };
            var entity = await _histories.AddAsync(dto);

            // fill attempts
            foreach(var attempt in command.Attempts)
            {
                await _attempts.AddAsync(new AttemptDto()
                {
                    HistoryId = entity.Id,
                    ExpectedTranslations = string.Join(",", attempt.ExpectedTranslations),
                    Word = attempt.Word,
                    UserTranslation = attempt.UserTranslation,
                    IsCorrect = attempt.IsCorrect,
                    TotalSeconds = attempt.TotalSeconds,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                });
            }

            return StateResult.Success(entity.Id);
        }
    }
}
