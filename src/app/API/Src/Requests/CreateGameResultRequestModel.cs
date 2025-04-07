using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Src.Requests
{
    public class CreateGameResultRequestModel
    {
        public ulong UserId { get; set; } 

        public WordGameResultModel[] Results { get; set; }

        public ulong MaxStreak { get; set; }

    }
}
