using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;

namespace Business.Src.Commands
{
    public class GetAttemptsHistoryCommand : IRequest<SelectResult<AttemptHistoryModel>>
    {
       
    }
}
