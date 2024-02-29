using Objects.Src.Models;

namespace API.Src.Requests
{
    public class CreateAttemptHistoryRequestModel
    {
        public ulong UserId { get; set; }

        public ulong TotalWords { get; set; }

        public ulong CorrectAttempts { get; set; }

        public double TotalSeconds { get; set; }

        public AttemptModel[] Attempts { get; set; }  
    }
}
