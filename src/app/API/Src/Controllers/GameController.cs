using API.Src.Requests;
using API.View;
using Business.Commands;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Objects.Models;
using Objects.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Src.Controllers
{
    [ApiController, Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<ActionResult> CreateGameResultAsync([FromBody] CreateGameResultRequestModel request)
        {
            var response = await _mediator.Send(new CreateGameResultCommand()
            {
                UserId = request.UserId,
                Results = request.Results,
                MaxStreak = request.MaxStreak
            });

            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<GameAttemptModel>>> GetGamesAsync(ulong? userId)
        {
            var response = await _mediator.Send(new GetGamesHistoryCommand() { UserId = userId});

            return PageViewModel<GameAttemptModel>.New(response.Data);
        }
    }
}
