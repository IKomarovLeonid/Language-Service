using Domain.Src;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects.Src.Dto;
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
            WordType wordType = WordType.Any;
            WordCategory wordCategory = WordCategory.Any;

            using (StreamReader sr = new StreamReader("words_esp.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if (line.Contains("["))
                    {
                        line = line.Substring(1, line.IndexOf("]") - 1);
                        var headerData = line.Split(',');
                        if (headerData.Length == 2)
                        {
                            wordCategory = headerData[1] switch
                            {
                                "colors" => WordCategory.Colors,
                                "time" => WordCategory.Time,
                                "directions" => WordCategory.Directions,
                                "character" => WordCategory.Character,
                                "family" => WordCategory.Family,
                                "common" => WordCategory.Any,
                                "house" => WordCategory.House,
                                "food" => WordCategory.Food,
                                "office" => WordCategory.Office,
                                "human" => WordCategory.Human,
                                "protection" => WordCategory.Protection,
                                _ => WordCategory.Any,
                            };
                        }
                        if (headerData.Length == 1)
                        {
                            wordCategory = WordCategory.Any;
                        }

                        wordType = headerData[0] switch
                        {
                            "nouns" => WordType.Noun,
                            "adjectives" => WordType.Adjective,
                            "adverbs" => WordType.Adverb,
                            "verbs" => WordType.Verb,
                            "prepositions" => WordType.Preposition,
                            "pronoun" => WordType.Pronoun,
                            "questions" => WordType.Questions,
                            _ => WordType.Any,
                        };
                        continue;
                    }
                    string[] data = line.Split(';');
                    if (data.Length < 2) { throw new Exception($"Invalid line: {line}"); }

                    var word = data[0];
                    var translation = data[1];

                    words.Add(new WordDto()
                    {
                        CreatedTime = DateTime.UtcNow,
                        UpdatedTime = DateTime.UtcNow,
                        Word = word,
                        Translation = translation,
                        LanguageFrom = LanguageType.Spanish,
                        LanguageTo = LanguageType.Russian,
                        Type = wordType,
                        Category = wordCategory
                    });
                }
            }

            // triste
            // concluir
            // facIl
            // alegre
            // el patio
            // alrededor de
            // excepto
            // gris
            // sAbado
            // sin
            // negociar
            // apoyar
            // delante de
            // cerca de
            // plateado
            // aconsejar
            // la computadora
            // el ordenador
            // el entrenador
            // el monitor
            // bastante
            // hasta
            // desaynar

            ctx.Words.AddRange(words);
            await ctx.SaveChangesAsync();
        }
    }
}
