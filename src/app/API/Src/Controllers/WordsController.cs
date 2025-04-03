using System.Threading.Tasks;
using API.View;
using Business.Commands;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Models;
using Objects.Src.Models;

namespace API.Controllers
{
    [ApiController, Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordsController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<WordModel>>> GetWordsAsync(string queryBy)
        {
            var response = await _mediator.Send(new GetWordsCommand(queryBy));

            return PageViewModel<WordModel>.New(response.Data);
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<PageViewModel<WordStatisticsModel>>> GetWordStatisticsAsync(ulong? userId)
        {
            var response = await _mediator.Send(new GetWordStatisticsCommand() { UserId = userId });

            return PageViewModel<WordStatisticsModel>.New(response.Data);
        }
    }
}
