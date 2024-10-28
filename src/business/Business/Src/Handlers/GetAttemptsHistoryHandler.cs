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

        public GetAttemptsHistoryHandler(IRepository<AttemptHistoryDto> histories)
        {
            _histories = histories;
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
                    AttemptsTotal = dto.TotalAttempts,
                    CorrectAttempts = dto.CorrectAttempts,
                    ErrorsTotal = dto.TotalAttempts - dto.CorrectAttempts,
                    SuccessRate = dto.TotalAttempts == 0 ? 0 :
                    (double) dto.CorrectAttempts * 100 / dto.TotalAttempts,
                    UserId = dto.UserId,
                    Errors = new Dictionary<string, uint>(),
                    CreatedTime = dto.CreatedTime,
                    UpdatedTime = dto.UpdatedTime
                };

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
