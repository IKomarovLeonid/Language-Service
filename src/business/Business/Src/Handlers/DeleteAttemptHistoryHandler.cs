
using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using Objects.Src.Models;
using System.Threading;
using System.Threading.Tasks;
using Objects.Dto;

namespace Business.Src.Handlers
{
    internal class DeleteAttemptHistoryHandler : IRequestHandler<DeleteAttemptHistoryCommand, StateResult>
    {
        private readonly IRepository<AttemptHistoryDto> _histories;

        public DeleteAttemptHistoryHandler(IRepository<AttemptHistoryDto> histories)
        {
            _histories = histories;
        }

        public async Task<StateResult> Handle(DeleteAttemptHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = await _histories.FindByIdAsync(request.Id);
            if (history == null) return StateResult.Error("Attempt does not exists");
            await _histories.DeleteAsync(request.Id);
            return StateResult.Success(request.Id);
        }
    }
}
