
using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using Objects.Src.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Src.Handlers
{
    internal class DeleteAttemptHistoryHandler : IRequestHandler<DeleteAttemptHistoryCommand, StateResult>
    {
        private readonly IRepository<AttemptHistoryDto> _histories;
        private readonly IRepository<AttemptDto> _attempts;

        public DeleteAttemptHistoryHandler(IRepository<AttemptHistoryDto> histories, IRepository<AttemptDto> attempts)
        {
            _histories = histories;
            _attempts = attempts;
        }

        public async Task<StateResult> Handle(DeleteAttemptHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = await _histories.FindByIdAsync(request.Id);
            if (history == null) return StateResult.Error("Attempt does not exists");
            // clear attempts first
            var attempts = await _attempts.GetAllAsync(a => a.HistoryId == history.Id);
            foreach (var attempt in attempts) { await _attempts.DeleteAsync(attempt.Id); }
            await _histories.DeleteAsync(request.Id);
            return StateResult.Success(request.Id);
        }
    }
}
