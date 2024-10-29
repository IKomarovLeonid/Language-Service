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
using Domain;

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
            var spanish = ReadWords("words_esp.txt", LanguageType.SpanishRussian).ToList();
            var english = ReadWords("words_eng.txt", LanguageType.EnglishRussian).ToList();

            ctx.Words.AddRange(spanish);
            ctx.Words.AddRange(english);

            PrintDuplicates(spanish);
            PrintDuplicates(english);

            await ctx.SaveChangesAsync();
        }

        private IEnumerable<WordDto> ReadWords(string fileName, LanguageType language)
        {
            var words = new List<WordDto>();
            string attributes = null;

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
                            continue;
                        }
                        string[] data = line.Split(';');
                        if (data.Length < 2) { throw new Exception($"Invalid line: {line}"); }

                        var word = data[0];
                        var translation = data[1];
                        var conjugation = data.Length > 2 ? data[2] : null;

                        words.Add(new WordDto()
                        {
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                            Word = word,
                            Attributes = attributes,
                            Translation = translation,
                            Conjugation = conjugation,
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
