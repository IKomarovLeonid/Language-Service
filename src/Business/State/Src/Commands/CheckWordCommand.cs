

using MediatR;
using Service.Models;

namespace State.Src.Commands
{
    public class CheckWordCommand : IRequest<CheckResult>
    {
        public Language from { get; set; }
        public string FromValue { get; set; }
        public Language To { get; set; }
        public string ToValue { get; set; }
        public WordsType Type { get; set; }
    }
}
