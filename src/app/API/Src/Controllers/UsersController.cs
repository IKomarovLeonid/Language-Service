using API.Src.Requests;
using API.View;
using Business.Commands;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Models;
using Objects.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Src.Controllers
{
    [ApiController, Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<ActionResult> RegisterUserAsync([FromBody] RegisterUserRequestModel request)
        {
            var response = await _mediator.Send(new CreateGameResultCommand()
            {
                UserId = 2
            });

            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserModel>>> GetUsersAsync(ulong id)
        {
            var response = await _mediator.Send(new GetUserCommand(id));

            return new ActionResult<ICollection<UserModel>>(response.Data);
        }
    }
}
