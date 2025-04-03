using Business.Src.Objects;
using MediatR;
using Objects.Models;

namespace Business.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordModel>>
    {
        public readonly string FilterBy;

        public GetWordsCommand(string filterBy)
        {
            FilterBy = filterBy;
        }
    }
}
