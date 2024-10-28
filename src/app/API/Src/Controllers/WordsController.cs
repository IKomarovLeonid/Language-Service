using API.Src.View;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Src.Models;
using System.Threading.Tasks;

namespace API.Src.Controllers
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
    }
}
