using Domain.Src;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects.Src;
using Objects.Src.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Src
{
    internal class HostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedService(IServiceScopeFactory factory)
        {
            _scopeFactory = factory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await MigrateAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        private async Task MigrateAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var isCreated = await db.Database.EnsureCreatedAsync(cancellationToken);
            // fill default data
            if (isCreated) await FillPredefinedDataAsync(db);
        }

        private async Task FillPredefinedDataAsync(ApplicationContext ctx)
        {
            var words = new List<WordDto>
            {
                new WordDto { LanguageFrom = LanguageType.Spanish, LanguageTo = LanguageType.English, Word = "azul", Translation = "blue", Type = WordType.Noun, CreatedTime = DateTime.UtcNow, UpdatedTime = DateTime.Now },
                new WordDto { LanguageFrom = LanguageType.Spanish, LanguageTo = LanguageType.English, Word = "verde", Translation = "green", Type = WordType.Noun, CreatedTime = DateTime.UtcNow, UpdatedTime = DateTime.Now },
                new WordDto { LanguageFrom = LanguageType.Spanish, LanguageTo = LanguageType.English, Word = "rojo", Translation = "red", Type = WordType.Noun, CreatedTime = DateTime.UtcNow, UpdatedTime = DateTime.Now }
            };

            ctx.Words.AddRange(words);
            await ctx.SaveChangesAsync();
        }
    }
}
