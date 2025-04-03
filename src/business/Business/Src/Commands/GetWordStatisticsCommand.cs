using Business.Src.Objects;
using MediatR;
using Objects.Models;
using Objects.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Src.Commands
{
    public class GetWordStatisticsCommand : IRequest<SelectResult<WordStatisticsModel>>
    {
        public ulong? UserId { get; set; }
    }
}
