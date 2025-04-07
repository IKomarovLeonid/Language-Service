using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Src.Dto
{
    public class WordStatisticsDto : IDto
    {
        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public ulong WordId { get; set; }

        public ulong CorrectAnswersTotal { get; set; }

        public ulong TotalAnswersCount { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public double GetSuccessRate() => this.TotalAnswersCount > 0 ? (double)(this.CorrectAnswersTotal / (double)this.TotalAnswersCount) * 100 : 0;
    }
}
