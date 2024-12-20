﻿using System.Threading.Tasks;
using API.Requests;
using API.View;
using Business.Commands;
using Business.Src.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Src.Models;

namespace API.Controllers
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
                CorrectAttempts = request.CorrectAttempts,
                TotalAttempts = request.TotalAttempts,
                WordErrors = request.WordErrors,
                AttemptAttributes = request.AttemptAttributes
            });

            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttemptHistoryAsync(ulong id)
        {
            var response = await _mediator.Send(new DeleteAttemptHistoryCommand(id));
            return response.IsSuccess ? Ok() : BadRequest(response);
        }
    }
}
