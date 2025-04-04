﻿using System;
using Objects.Src;

namespace Objects.Dto
{
    public class GameAttemptDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public double UserRatingChange { get; set; }

        public ulong CorrectAnswersCount { get; set; }

        public ulong TotalAnswersCount { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
