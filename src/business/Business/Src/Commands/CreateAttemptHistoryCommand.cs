using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;

namespace Business.Src.Commands
{
    public class CreateAttemptHistoryCommand : IRequest<StateResult>
    {
        public ulong UserId { get; set; }

        public ulong TotalWords { get; set; }

        public ulong CorrectAttempts { get; set; }

        public double TotalSeconds { get; set; }

        public AttemptModel[] Attempts { get; set; }
    }
}
