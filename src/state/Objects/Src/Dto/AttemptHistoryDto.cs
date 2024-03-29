﻿using Objects.Src.Primitives;
using System;

namespace Objects.Src.Models
{
    public class AttemptHistoryDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public WordType WordTypes { get; set; }

        public WordCategory Category { get; set; }

        public double TotalSeconds { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
