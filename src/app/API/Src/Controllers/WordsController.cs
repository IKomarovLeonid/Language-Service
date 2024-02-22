using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Src.Primitives;
using System.Threading.Tasks;

namespace API.Src.Controllers
{
    [ApiController, Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordsController(IMediator mediator) { _mediator = mediator; }


        [HttpGet]
        public async Task<IActionResult> GetWordsAsync(WordCategory? category, WordType? type, WordLevel? level, LanguageType? from, LanguageType? to)
        {
            return Ok(_mediator.Send(new GetWordsCommand(category, type, from, to, level)));
        }

        [HttpPost]
        public async Task<IActionResult> AddWordAsync()
        {
            return Ok("word added");
        }


        [HttpPatch]
        public async Task<IActionResult> UpdateWordAsync()
        {
            return Ok("word updated");
        }
    }
}
