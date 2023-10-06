using System;

namespace State.Src.Commands
{
    public class CreateReportRequestModel
    {
        public ulong UserId { get; set; }

        public ulong TotalCount { get; set; }

        public ulong ErrorsCount { get; set; }

        public DateTime Date { get; set; }
    }
}
