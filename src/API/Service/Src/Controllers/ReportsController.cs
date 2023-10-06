using MediatR;
using Microsoft.AspNetCore.Mvc;
using State.Src.Commands;
using System.Threading.Tasks;

namespace Service.Src.Controllers
{
    [ApiController, Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(ulong id)
        {
            return Ok(1);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateReportRequestModel request)
        {
            return Ok(1);
        }

    }
}
