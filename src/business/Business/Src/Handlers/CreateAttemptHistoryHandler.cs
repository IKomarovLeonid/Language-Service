using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using Objects.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                TotalWords = command.TotalWords,
                TotalSeconds = command.TotalSeconds,
                CorrectAttempts = command.CorrectAttempts,
                UserId = command.UserId
            };
            var entity = await _histories.AddAsync(dto);

            // fill attempts
            foreach(var attempt in command.Attempts)
            {
                await _attempts.AddAsync(new AttemptDto()
                {
                    HistoryId = entity.Id,
                    ExpectedTranslation = attempt.ExpectedTranslation,
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
