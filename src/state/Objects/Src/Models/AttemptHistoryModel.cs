using Objects.Src.Primitives;
using System;
using System.Collections.Generic;

namespace Objects.Src.Models
{
    public class AttemptHistoryModel
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public ulong AttemptsTotal { get; set; }

        public ulong CorrectAttempts { get; set; }

        public ulong ErrorsTotal { get; set; }

        public double SuccessRate { get; set; }

        public string WordErrors { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
