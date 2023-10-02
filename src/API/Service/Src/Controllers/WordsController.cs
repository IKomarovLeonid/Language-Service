using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Src.Models;
using State.Src;
using State.Src.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Src.Controllers
{
    [ApiController, Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Language from, Language to, WordsType type)
        {
            var result = await _mediator.Send(new GetWordsCommand(from, to, type));
            return Ok(result);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomAsync(Language from, Language to, WordsType type)
        {
            var result = await _mediator.Send(new GetWordsCommand(from, to, type));
            return Ok(result.ElementAt(new Random().Next(result.Count)));
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckAsync([FromBody] CheckWordRequestModel request)
        {
            var result = await _mediator.Send(new CheckWordCommand()
            {
                To = request.To,
                FromValue = request.FromValue,
                ToValue = request.ToValue,
                Type = request.Type,
                from = request.from
            });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}
