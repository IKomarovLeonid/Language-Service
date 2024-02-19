using Business.Src.Objects;
using MediatR;
using Objects.Src;

namespace Business.Src.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordDto>>
    {

    }
}
