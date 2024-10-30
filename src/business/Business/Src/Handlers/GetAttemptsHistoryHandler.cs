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
using Business.Commands;
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
                    WordErrors = dto.WordsErrors,
                    AttemptAttributes = dto.AttemptAttributes,
                    CreatedTime = dto.CreatedTime,
                    UpdatedTime = dto.UpdatedTime
                };
                models.Add(model);
            }

            return SelectResult<AttemptHistoryModel>.Fetched(models.OrderByDescending(t => t.CreatedTime).ToList());
        }
    }
}
