using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Objects;
using Objects.Dto;

namespace API
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
            var english = ReadWords("words_eng.txt", WordLanguageType.EnglishRussian).ToList();
            var spanish = ReadWords("words_esp.txt", WordLanguageType.SpanishRussian).ToList();

            ctx.Words.AddRange(spanish);
            ctx.Words.AddRange(english);

            PrintDuplicates(spanish);
            PrintDuplicates(english);

            await ctx.SaveChangesAsync();

        }

        private IEnumerable<WordDto> ReadWords(string fileName, WordLanguageType type)
        {
            var words = new List<WordDto>();
            string attributes = null;

            var categories = new Dictionary<string, List<string>>();
            string currentCategory = null;

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
                            attributes = line.Substring(1, line.IndexOf("]") - 1);
                            currentCategory = line.Trim();
                            continue;
                        }
                        if (currentCategory != null)
                        {
                            if (categories.ContainsKey(currentCategory)) {
                                categories[currentCategory].Add(line);
                            }
                            else
                            {
                                 categories.Add(currentCategory, new List<string>() { line });
                            }
                        }

                        var data = line.Split(';');
                        if (data.Length < 2) { throw new Exception($"Invalid line: {line}"); }

                        var word = data[0];
                        var translation = data[1];
                        var conjugation = data.Length > 2 ? data[2] : null;

                        words.Add(new WordDto()
                        {
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                            Word = word,
                            LanguageType = type,
                            Attributes = attributes,
                            Translation = translation,
                            Conjugation = conjugation,
                        });
                    }
                }
                Console.WriteLine($"Processed {words.Count} words from source {fileName}");

                var name = "sort_" + fileName;
                using (StreamWriter writer = new StreamWriter(name))
                {
                    foreach (var category in categories)
                    {
                        writer.WriteLine(category.Key);

                        var sortedWords = category.Value
                            .OrderBy(item => item.Split(';')[0])
                            .ToList();

                        foreach (var word in sortedWords)
                        {
                            writer.WriteLine(word);
                        }

                        writer.WriteLine();
                    }
                }

                Console.WriteLine($"New sorted file has been created {name}");

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
