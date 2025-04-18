﻿using API.Src.Requests;
using Business.Src.Objects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Src.Commands
{
    public class CreateGameResultCommand : IRequest<StateResult>
    {
        public ulong UserId { get; set; }

        public WordGameResultModel[] Results { get; set; }

        public ulong MaxStreak { get; set; }
    }
}
