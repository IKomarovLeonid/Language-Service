using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Src.Dto
{
    public class UserStatisticsDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public double UserRating { get; set; }

        public ulong TotalAttempts { get; set; }

        public ulong ErrorAttempts { get; set; }

        public DateTime LastAttempt { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
