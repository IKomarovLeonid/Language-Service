using Business.Src.Objects;
using MediatR;
using Objects.Src.Models;

namespace Business.Commands
{
    public class GetAttemptsHistoryCommand : IRequest<SelectResult<AttemptHistoryModel>>
    {
       
    }
}
