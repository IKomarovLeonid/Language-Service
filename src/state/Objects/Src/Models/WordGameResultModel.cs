using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Src.Requests
{
    public class WordGameResultModel
    {
        public ulong WordId { get; set; }

        public ulong CorrectCount { get; set; }

        public ulong TotalCount { get; set; }
    }
}
