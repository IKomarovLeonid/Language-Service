using MediatR;
using Objects.Src;
using State.Src.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace State.Src.Handlers
{
    internal class CheckWordHandler : IRequestHandler<CheckWordCommand, CheckResult>
    {
        private readonly WordsRepository _wordsRepository;
        public CheckWordHandler(WordsRepository repository)
        {
            _wordsRepository = repository;
        }

        public async Task<CheckResult> Handle(CheckWordCommand request, CancellationToken cancellationToken)
        {
            var fromValue = request.FromValue.ToLowerInvariant();
            var toValue = request.ToValue.ToLowerInvariant();

            var words = _wordsRepository.GetWords(request.Type);
            if (words.Count == 0) return CheckResult.Error($"Empty words by type '{request.Type}'");

            if (!words.TryGetValue(fromValue, out string value))
            {
                return CheckResult.Error($"Word '{request.FromValue}' not found");
            }
            if(toValue != value.ToLowerInvariant())
            {
                return CheckResult.Error($"Wrong answer. Correct is: {value}");
            }
            return CheckResult.Success();

        }
    }
}
