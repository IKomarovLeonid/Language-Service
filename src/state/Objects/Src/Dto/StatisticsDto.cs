using System;

namespace Objects.Src.Dto
{
    public class StatisticsDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }   

        public ulong TotalAttempts { get; set; }

        public ulong SuccessAttempts { get; set; }

        public double Percent { get; set; }

        public string Description { get; set; } 

        public ulong TotalTimeSeconds { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
