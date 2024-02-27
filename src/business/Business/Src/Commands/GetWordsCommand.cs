using Business.Src.Objects;
using MediatR;
using Objects.Src.Primitives;
using Objects.Src.Models;

namespace Business.Src.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordModel>>
    {
        public GetWordsCommand() {}
    }
}
