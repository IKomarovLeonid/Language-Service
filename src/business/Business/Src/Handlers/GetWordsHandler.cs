using Business.Src.Commands;
using Business.Src.Objects;
using Domain.Src;
using MediatR;
using NJsonSchema.Validation;
using Objects.Src;
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
            Expression<Func<WordDto, bool>> expression = null;
            if (request.Type.HasValue && request.LanguageFrom.HasValue && request.LanguageTo.HasValue)
            {
                expression = model =>
                model.Type == request.Type.Value && model.LanguageFrom == request.LanguageFrom.Value &&
                model.LanguageTo == request.LanguageTo.Value;
            }
            if (request.Type.HasValue)
            {
                expression = model =>
                model.Type == request.Type.Value;
            }
            var items = await _repository.GetAllAsync(expression);
            return SelectResult<WordDto>.Fetched(items);
        }
    }
}
