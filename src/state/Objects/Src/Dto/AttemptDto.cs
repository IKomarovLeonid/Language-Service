using System;

namespace Objects.Src.Dto
{
    public class AttemptDto : IDto
    {
        public ulong Id { get; set; }

        public ulong HistoryId { get; set; }

        public string Word { get; set; }

        public string UserTranslation { get; set; }

        public string ExpectedTranslations { get; set; }

        public bool IsCorrect { get; set; }

        public double TotalSeconds { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

    }
}
