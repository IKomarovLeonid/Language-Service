using API.Src.Requests;
using API.Src.View;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Src.Models;
using System.Threading.Tasks;

namespace API.Src.Controllers
{
    [ApiController, Route("api/history")]
    public class HistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HistoryController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<AttemptHistoryModel>>> GetAttemptsAsync()
        {
            var response = await _mediator.Send(new GetAttemptsHistoryCommand());

            return PageViewModel<AttemptHistoryModel>.New(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAttemptHistoryAsync([FromBody] CreateAttemptHistoryRequestModel request)
        {
            var response = await _mediator.Send(new CreateAttemptHistoryCommand()
            {
                Attempts = request.Attempts,
                CorrectAttempts = request.CorrectAttempts,
                TotalSeconds = request.TotalSeconds,
                TotalAttempts = request.TotalAttempts,
                WordTypes = request.WordTypes,
                Category = request.Category
            });

            return response.IsSuccess ? Ok() : BadRequest(response);
        }
    }
}
