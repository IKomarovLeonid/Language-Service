using System;

namespace Objects.Src.Models
{
    public class AttemptHistoryModel
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public ulong TotalWords { get; set; }

        public ulong CorrectAttempts { get; set; }

        public ulong Errors { get; set; }

        public double TotalSeconds { get; set; }

        public double Percent { get; set; }

        public double AvgAnswerTimeSec { get; set; }

        public AttemptModel[] Attempts { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
