using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using Objects.Dto;
using Objects.Models;

namespace Business.Handlers
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
                WordRating = dto.WordRating,
                Attributes = dto.Attributes,
                LanguageType = dto.LanguageType,
                Conjugation = dto.Conjugation,
                CreatedTime = dto.CreatedTime,
                UpdatedTime = dto.UpdatedTime,
            }).ToList());
        }
    }
}
