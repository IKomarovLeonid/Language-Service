using Business.Src.Objects;
using MediatR;
using Objects.Src;
using Objects.Src.Primitives;

namespace Business.Src.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordDto>>
    {
        public GetWordsCommand(WordType? type, LanguageType? from, LanguageType? to) {
            LanguageTo = to;
            LanguageFrom = from;
            Type = type;
        }

        public WordType? Type { get; private set; }

        public LanguageType? LanguageFrom { get; private set; }  

        public LanguageType? LanguageTo { get; private set; }
    }
}
