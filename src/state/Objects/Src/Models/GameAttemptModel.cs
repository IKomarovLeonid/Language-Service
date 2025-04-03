using System;
using System.Collections.Generic;

namespace Objects.Src.Models
{
    public class GameAttemptModel
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public double UserRatingChange { get; set; }

        public double TotalAnswers { get; set; }

        public double ErrorAnswers { get; set; }

        public double SuccessRate { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
