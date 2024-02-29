using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;
using Objects.Src.Primitives;

namespace Business.Src.Commands
{
    public class CreateAttemptHistoryCommand : IRequest<StateResult>
    {
        public ulong UserId { get; set; }

        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public double TotalSeconds { get; set; }

        public WordType? WordTypes { get; set; }

        public WordCategory? Category { get; set; }

        public AttemptModel[] Attempts { get; set; }
    }
}
