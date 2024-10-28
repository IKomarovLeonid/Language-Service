using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Src.Dto;
using Objects.Src.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Src.Handlers
{
    internal class GetWordsHandler : IRequestHandler<GetWordsCommand, SelectResult<WordModel>>
    {
        private readonly IRepository<WordDto> _repository;

        public GetWordsHandler(IRepository<WordDto> repository)
        {
            _repository = repository;
        }

        public async Task<SelectResult<WordModel>> Handle(GetWordsCommand request, CancellationToken cancellationToken)
        {
            var items = request.FilterBy == null ?
                await _repository.GetAllAsync() :
                await _repository.GetAllAsync(t => t.Attributes.Contains(request.FilterBy));

            return SelectResult<WordModel>.Fetched(items.Select(dto => new WordModel()
            {
                Id = dto.Id,
                Translations = dto.Translation.Split(","),
                Word = dto.Word,
                Attributes = dto.Attributes,
                Conjugation = dto.Conjugation,
                CreatedTime = dto.CreatedTime,
                UpdatedTime = dto.UpdatedTime,
            }).ToList());
        }
    }
}
