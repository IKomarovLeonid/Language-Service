﻿using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Src.Handlers
{
    internal class GetWordsHandler : IRequestHandler<GetWordsCommand, SelectResult<WordDto>>
    {
        private readonly IRepository<WordDto> _repository;

        public GetWordsHandler(IRepository<WordDto> repository)
        {
            _repository = repository;
        }

        public async Task<SelectResult<WordDto>> Handle(GetWordsCommand request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync(request.Filter);
            return SelectResult<WordDto>.Fetched(items);
        }
    }
}
