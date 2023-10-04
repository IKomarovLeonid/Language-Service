using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Src.Models;
using State.Src;
using State.Src.Commands;
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
        public async Task<WordsViewModel> GetAsync(Language from, Language to, WordsType type)
        {
            var result = await _mediator.Send(new GetWordsCommand(from, to, type));
            return new WordsViewModel(result, from, to, type);
        }

    }
}
