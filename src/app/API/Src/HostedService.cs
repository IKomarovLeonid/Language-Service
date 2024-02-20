using Domain.Src;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects.Src;
using Objects.Src.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            WordType wordType = WordType.Undefined;
            WordCategory wordCategory = WordCategory.Common;

            using (StreamReader sr = new StreamReader("words_esp.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.Contains("["))
                    {
                        line = line.Substring(1, line.IndexOf("]") - 1);
                        var headerData = line.Split(',');
                        if (headerData.Length > 1)
                        {
                            wordCategory = headerData[1] switch
                            {
                                "colors" => WordCategory.Colors,
                                "time" => WordCategory.Time,
                                "directions" => WordCategory.Directions,
                                _ => WordCategory.Common,
                            };
                        }
                        else wordCategory = WordCategory.Common;

                        wordType = headerData[0] switch
                        {
                            "nouns" => WordType.Noun,
                            "prepositions" => WordType.Pronoun,
                            "adjectives" => WordType.Adjective,
                            "adverbs" => WordType.Adverb,
                            _ => WordType.Undefined,
                        };
                        continue;
                    }
                   
                    string[] data = line.Split(';');
                    if(data.Length < 2) { throw new Exception($"Invalid line: {line}"); }

                    var word = data[0];
                    var translation = data[1];
                    
                    words.Add(new WordDto()
                    {
                        CreatedTime = DateTime.UtcNow,
                        UpdatedTime = DateTime.UtcNow,
                        Word = word,
                        Translation = translation,
                        LanguageFrom = LanguageType.Spanish,
                        LanguageTo = LanguageType.English,
                        Type = wordType,
                        Category = wordCategory
                    });
                }
            }

            ctx.Words.AddRange(words);
            await ctx.SaveChangesAsync();
        }
    }
}
