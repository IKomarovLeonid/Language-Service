using Objects.Src.Models;
using Objects.Src.Primitives;

namespace API.Src.Requests
{
    public class CreateAttemptHistoryRequestModel
    {
        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public double TotalSeconds { get; set; }

        public AttemptModel[] Attempts { get; set; }

        public WordType? WordTypes { get; set; }

        public WordCategory? Category { get; set; }
    }
}
