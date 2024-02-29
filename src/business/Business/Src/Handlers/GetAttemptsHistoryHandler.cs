using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src;
using Objects.Src.Dto;
using Objects.Src.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Src.Handlers
{
    internal class GetAttemptsHistoryHandler : IRequestHandler<GetAttemptsHistoryCommand, SelectResult<AttemptHistoryModel>>
    {
        private readonly IRepository<AttemptHistoryDto> _histories;
        private readonly IRepository<AttemptDto> _attempts;

        public GetAttemptsHistoryHandler(IRepository<AttemptHistoryDto> histories, IRepository<AttemptDto> attempts)
        {
            _histories = histories;
            _attempts = attempts;
        }


        public async Task<SelectResult<AttemptHistoryModel>> Handle(GetAttemptsHistoryCommand request, CancellationToken cancellationToken)
        {
            var items = await _histories.GetAllAsync();
            var models = new List<AttemptHistoryModel>();

            foreach (var dto in items)
            {
                var model = new AttemptHistoryModel()
                {
                    Id = dto.Id,
                    TotalSeconds = dto.TotalSeconds,
                    TotalWords = dto.TotalWords,
                    CorrectAttempts = dto.CorrectAttempts,
                    Errors = dto.TotalWords - dto.CorrectAttempts,
                    AvgAnswerTimeSec = dto.CorrectAttempts == 0 ? dto.TotalSeconds / (dto.TotalWords - dto.CorrectAttempts) :
                    dto.TotalSeconds / dto.CorrectAttempts,
                    Percent = dto.TotalWords == 0 ? 0 :
                    dto.CorrectAttempts / dto.TotalWords,
                    UserId = dto.UserId,
                    CreatedTime = dto.CreatedTime,
                    UpdatedTime = dto.UpdatedTime
                };
                var attempts = await _attempts.GetAllAsync(a => a.HistoryId == dto.Id);
                model.Attempts = attempts.Select(aDto => new AttemptModel()
                {
                    IsCorrect = aDto.IsCorrect,
                    ExpectedTranslation = aDto.ExpectedTranslation,
                    TotalSeconds = aDto.TotalSeconds,
                    UserTranslation = aDto.UserTranslation,
                    Word = aDto.Word
                }).ToArray();
                models.Add(model);
            }

            return SelectResult<AttemptHistoryModel>.Fetched(models);
        }
    }
}
