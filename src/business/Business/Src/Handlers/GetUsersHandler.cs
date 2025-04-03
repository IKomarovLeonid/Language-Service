using Business.Commands;
using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
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

        public GetUsersHandler(IRepository<UserDto> users)
        {
            _users = users;
        }

        public async Task<SelectResult<UserModel>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _users.FindByIdAsync(request.UserId);
            if (userDto == null) return SelectResult<UserModel>.Error("User not found");

            return SelectResult<UserModel>.Fetched(new List<UserModel>() { new UserModel()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Id = userDto.Id,
                IsAdmin = userDto.IsAdmin,
                UpdatedTime = userDto.UpdatedTime
            }});
        }
    }
}
