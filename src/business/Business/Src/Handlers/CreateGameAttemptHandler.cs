using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands;
using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Dto;
using Objects.Src.Dto;

namespace Business.Handlers
{
    internal class CreateGameAttemptHandler : IRequestHandler<CreateGameResultCommand, StateResult>
    {
        private readonly IRepository<GameAttemptDto> _games;
        private readonly IRepository<UserDto> _users;
        private readonly IRepository<WordStatisticsDto> _statistics;

        public CreateGameAttemptHandler(IRepository<GameAttemptDto> games, IRepository<UserDto> users, IRepository<WordStatisticsDto> wordStatistics)
        {
            _games = games;
            _users = users;
            _statistics = wordStatistics;
        }

        public async Task<StateResult> Handle(CreateGameResultCommand command, CancellationToken cancellationToken)
        {
            var userDto = await _users.FindByIdAsync(command.UserId);
            if (userDto == null) return StateResult.Error($"User does not exists {command.UserId}");
            if (command.Results == null || command.Results.Length == 0) return StateResult.Error("Results are empty");

            var correctCount = 0UL;
            var totalCount = 0UL;

            var statistics = await _statistics.GetAllAsync(s => s.UserId == command.UserId);
            foreach(var stats in command.Results)
            {
                totalCount += stats.TotalCount;
                correctCount += stats.CorrectCount;

                var existedDto = statistics.FirstOrDefault(s => s.WordId == stats.WordId);
                if (existedDto == null) await _statistics.AddAsync(new WordStatisticsDto()
                {
                    CorrectAnswersTotal = stats.CorrectCount,
                    TotalAnswersCount = stats.TotalCount,
                    UserId = command.UserId,
                    WordId = stats.WordId,
                });
                else
                {
                    existedDto.TotalAnswersCount += stats.TotalCount;
                    existedDto.CorrectAnswersTotal += stats.CorrectCount;
                    await _statistics.UpdateAsync(existedDto);
                }
            }

            if (totalCount < correctCount) return StateResult.Error($"Failed, wrong stats. Total {totalCount}, correct {correctCount}");

            var result = await _games.AddAsync(new GameAttemptDto()
            {
                UserId = command.UserId,
                UserRatingChange = 10,
                TotalAnswersCount = totalCount,
                CorrectAnswersCount = correctCount,
                MaxStreak = command.MaxStreak
            });

            return StateResult.Success(result.Id);
        }
    }
}
