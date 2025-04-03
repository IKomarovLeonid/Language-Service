using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Src.Models
{
    public class WordStatisticsModel
    {
        public ulong Id { get; set; }

        public ulong? UserId { get; set; }

        public ulong WordId { get; set; }

        public ulong CorrectAnswersTotal { get; set; }

        public ulong TotalAnswersCount { get; set; }

        public double SuccessRate { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
