using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;

namespace Business.Commands
{
    public class CreateAttemptHistoryCommand : IRequest<StateResult>
    {
        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public string WordErrors { get; set; }

        public string AttemptAttributes { get; set; }
    }
}
