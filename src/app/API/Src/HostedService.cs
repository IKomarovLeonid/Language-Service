using Domain.Src;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects.Src;
using Objects.Src.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
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
            var words = new List<WordDto>();
            using (StreamReader sr = new StreamReader("words_esp.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // skip comment lines
                    if (line.Contains("/")) continue;
                   
                    string[] data = line.Split(';');
                    if(data.Length < 2) { throw new Exception($"Invalid line: {line}"); }

                    var word = data[0];
                    var translation = data[1];
                    var type = data[2];
                    var wordType = WordType.Undefined;
                    wordType = type switch
                    {
                        "n" => WordType.Noun,
                        "p" => WordType.Pronoun,
                        _ => WordType.Undefined,
                    };
                    words.Add(new WordDto()
                    {
                        CreatedTime = DateTime.UtcNow,
                        UpdatedTime = DateTime.UtcNow,
                        Word = word,
                        Translation = translation,
                        LanguageFrom = LanguageType.Spanish,
                        LanguageTo = LanguageType.English,
                        Type = wordType
                    });
                }
            }
            
            ctx.Words.AddRange(words);
            await ctx.SaveChangesAsync();
        }
    }
}
