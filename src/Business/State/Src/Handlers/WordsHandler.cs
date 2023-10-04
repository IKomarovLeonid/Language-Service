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
        private readonly WordsRepository wordsRepository;

        public WordsHandler(WordsRepository repository)
        {
            this.wordsRepository = repository;
        }

        public async Task<Dictionary<string, string>> Handle(GetWordsCommand command, CancellationToken cancellationToken)
        {
            // TODO: language
            return wordsRepository.GetWords(command.WordsType);
        }
    }
}
