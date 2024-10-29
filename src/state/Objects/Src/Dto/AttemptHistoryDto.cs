using System;
using Objects.Src;

namespace Objects.Dto
{
    public class AttemptHistoryDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public string WordsErrors { get; set; }

        public string AttemptAttributes { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
