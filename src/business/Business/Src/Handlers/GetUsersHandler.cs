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
    class GetUsersHandler : IRequestHandler<GetUserCommand, SelectResult<UserModel>>
    {
        private readonly IRepository<UserDto> _users;
        private readonly IRepository<GameAttemptDto> _attempts;

        public GetUsersHandler(IRepository<UserDto> users, IRepository<GameAttemptDto> attempts)
        {
            _users = users;
            _attempts = attempts;
        }

        public async Task<SelectResult<UserModel>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _users.FindByIdAsync(request.UserId);
            if (userDto == null) return SelectResult<UserModel>.Error("User not found");

            var model = new UserModel()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Id = userDto.Id,
                IsAdmin = userDto.IsAdmin,
                UpdatedTime = userDto.UpdatedTime
            };

            var attempts = await _attempts.GetAllAsync(a => a.UserId == request.UserId);

            if (attempts.Any())
            {
                // all time stats
                model.TotalAttempts = attempts.Aggregate((ulong)0, (sum, a) => sum + a.TotalAnswersCount);
                model.AllTimeSuccessRate = attempts.Average(s => s.GetSuccessRate());
                model.MaxStreak = attempts.Max(s => s.MaxStreak);
                model.LastGame = attempts.Max(s => s.CreatedTime);
            }

            return SelectResult<UserModel>.Fetched(new List<UserModel>() { model});
        }
    }
}
