using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Service.Models;
using Objects.Src;
using State.Src.Commands;

namespace State.Src.Handlers
{
    internal class WordsHandler : IRequestHandler<GetWordsCommand, Dictionary<string, string>>
    {
        public WordsHandler()
        {

        }

        public async Task<Dictionary<string, string>> Handle(GetWordsCommand command, CancellationToken cancellationToken)
        {
            // TODO: language
            return WordsRepository.GetWords(command.WordsType);
        }
    }
}
