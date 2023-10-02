using MediatR;
using Service.Models;
using System.Collections.Generic;

namespace State.Src.Commands
{
    public class GetWordsCommand : IRequest<Dictionary<string, string>>
    {
        public WordsType WordsType { get; private set; }

        public Language From { get; private set; }

        public Language To { get; private set; }

        public GetWordsCommand(Language from, Language to, WordsType type)
        {
            From = from;
            To = to;
            WordsType = type;
        }
    }
}
