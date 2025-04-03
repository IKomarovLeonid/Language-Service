using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src;
using Objects.Src.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands;
using Objects.Dto;
using System;

namespace Business.Src.Handlers
{
    internal class GetGamesHistoryHandler : IRequestHandler<GetGamesHistoryCommand, SelectResult<GameAttemptModel>>
    {
        private readonly IRepository<GameAttemptDto> _games;

        public GetGamesHistoryHandler(IRepository<GameAttemptDto> games)
        {
            _games = games;
        }

        public async Task<SelectResult<GameAttemptModel>> Handle(GetGamesHistoryCommand request, CancellationToken cancellationToken)
        {
            ICollection<GameAttemptDto> data = null;
            if (!request.UserId.HasValue)
            {
                data = await _games.GetAllAsync();
            } else data = await _games.GetAllAsync(t => t.UserId == request.UserId.Value);

            var models = new List<GameAttemptModel>();

            foreach (var dto in data)
            {
                var model = new GameAttemptModel()
                {
                    Id = dto.Id,
                    UserId = dto.Id,
                    ErrorAnswers = dto.TotalAnswersCount - dto.CorrectAnswersCount,
                    TotalAnswers = dto.TotalAnswersCount,
                    UserRatingChange = dto.UserRatingChange,
                    UpdatedTime = dto.UpdatedTime
                };
                models.Add(model);
            }

            return SelectResult<GameAttemptModel>.Fetched(models.OrderByDescending(t => t.UpdatedTime).ToList());
        }
    }
}
