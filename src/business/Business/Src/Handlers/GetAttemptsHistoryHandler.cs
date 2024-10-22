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
using Objects.Dto;

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
                    AttemptsTotal = dto.TotalAttempts,
                    CorrectAttempts = dto.CorrectAttempts,
                    ErrorsTotal = dto.TotalAttempts - dto.CorrectAttempts,
                    AvgAnswerTimeSec = dto.CorrectAttempts == 0 ? dto.TotalSeconds / (dto.TotalAttempts - dto.CorrectAttempts) :
                    dto.TotalSeconds / dto.CorrectAttempts,
                    SuccessRate = dto.TotalAttempts == 0 ? 0 :
                    (double) dto.CorrectAttempts * 100 / dto.TotalAttempts,
                    UserId = dto.UserId,
                    Errors = new Dictionary<string, uint>(),
                    WordTypes = dto.WordTypes,
                    Category = dto.Category,
                    CreatedTime = dto.CreatedTime,
                    UpdatedTime = dto.UpdatedTime
                };
                var attempts = await _attempts.GetAllAsync(a => a.HistoryId == dto.Id);
                model.Attempts = attempts.Select(aDto => new AttemptModel()
                {
                    IsCorrect = aDto.IsCorrect,
                    ExpectedTranslations = aDto.ExpectedTranslations.Split(","),
                    TotalSeconds = aDto.TotalSeconds,
                    UserTranslation = aDto.UserTranslation,
                    Word = aDto.Word
                }).ToArray();
                models.Add(model);

                // calculate stats
                foreach(var attempt in model.Attempts)
                {
                    if (attempt.IsCorrect) continue;

                    if (model.Errors.ContainsKey(attempt.Word)) model.Errors[attempt.Word]++;
                    else model.Errors.Add(attempt.Word, 1);
                }
            }

            return SelectResult<AttemptHistoryModel>.Fetched(models.OrderByDescending(t => t.CreatedTime).ToList());
        }
    }
}
