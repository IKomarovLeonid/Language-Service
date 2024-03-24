using Domain.Src;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects.Src.Dto;
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
            var spanish = ReadWords("words_esp.txt", LanguageType.SpanishRussian);
            var english = ReadWords("words_eng.txt", LanguageType.EnglishRussian);

            ctx.Words.AddRange(spanish);
            ctx.Words.AddRange(english);

            PrintDuplicates(spanish);
            PrintDuplicates(english);

            await ctx.SaveChangesAsync();
        }

        private IEnumerable<WordDto> ReadWords(string fileName, LanguageType language)
        {
            var words = new List<WordDto>();
            WordType wordType = WordType.Any;
            WordCategory wordCategory = WordCategory.Any;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
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
                                "phrases" => WordType.Phrases,
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
                            Language = language,
                            Type = wordType,
                            Category = wordCategory
                        });
                    }
                }
                Console.WriteLine($"Processed {words.Count} words from source {fileName}");
                return words;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed for words from source {fileName} because of {ex.Message}");
                return words;
            }
            
        }

        private void PrintDuplicates(IEnumerable<WordDto> words)
        {
            var duplicates = words
                .GroupBy(obj => obj.Word)
                .Where(group => group.Count() > 1) 
                .SelectMany(group => group); 

            foreach (var duplicate in duplicates)
            {
                Console.WriteLine($"Duplicate Word: {duplicate.Word}");
            }
        }
    }
}
