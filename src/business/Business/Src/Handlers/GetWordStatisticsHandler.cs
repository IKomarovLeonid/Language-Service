using Business.Commands;
using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Dto;
using Objects.Models;
using Objects.Src.Dto;
using Objects.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Src.Handlers
{
    class GetWordStatisticsHandler : IRequestHandler<GetWordStatisticsCommand, SelectResult<WordStatisticsModel>>
    {
        private readonly IRepository<UserDto> _users;
        private readonly IRepository<WordStatisticsDto> _statistics;

        public GetWordStatisticsHandler(IRepository<UserDto> users, IRepository<WordStatisticsDto> wordStatistics)
        {
            _users = users;
            _statistics = wordStatistics;
        }

        public async Task<SelectResult<WordStatisticsModel>> Handle(GetWordStatisticsCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId.HasValue)
            {
                var userId = command.UserId.Value;
                var user = _users.FindByIdAsync(userId);
                if (user == null) return SelectResult<WordStatisticsModel>.Error($"User does not exists {userId}");
                // by user
                var stats = await _statistics.GetAllAsync(s => s.UserId == userId);
                var data = stats.Select(dto => new WordStatisticsModel()
                {
                    UserId = dto.UserId,
                    Id = dto.Id,
                    WordId = dto.WordId,
                    CorrectAnswersTotal = dto.CorrectAnswersTotal,
                    TotalAnswersCount = dto.TotalAnswersCount,
                    SuccessRate = dto.TotalAnswersCount == 0 ? 0 : ((double) dto.CorrectAnswersTotal / (double) dto.TotalAnswersCount) * 100,
                    UpdatedTime = dto.UpdatedTime
                }).ToList();
                return SelectResult<WordStatisticsModel>.Fetched(data);
            }

            var items = await _statistics.GetAllAsync();
            var dataAll = items.Select(dto => new WordStatisticsModel()
            {
                UserId = dto.UserId,
                Id = dto.Id,
                WordId = dto.WordId,
                CorrectAnswersTotal = dto.CorrectAnswersTotal,
                TotalAnswersCount = dto.TotalAnswersCount,
                SuccessRate = dto.TotalAnswersCount == 0 ? 0 : (double)(dto.CorrectAnswersTotal / (double)dto.TotalAnswersCount) * 100,
                UpdatedTime = dto.UpdatedTime
            }).ToList();
            return SelectResult<WordStatisticsModel>.Fetched(dataAll);
        }
    }
}
