using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Objects;
using Objects.Dto;
using Objects.Src.Dto;
using Objects.Src.Models;

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

            CreateAdmin(ctx);

            await ctx.SaveChangesAsync();

        }

        private IEnumerable<WordDto> ReadWords(string fileName, WordLanguageType type)
        {
            var words = new List<WordDto>();
            string attributes = null;

            var categories = new Dictionary<string, List<string>>();
            string currentCategory = null;

            if(type == WordLanguageType.SpanishRussian) LoadVerbos(words);

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
                            if (attributes == "End")
                            {
                                Console.WriteLine("Catch end");
                                break;
                            }
                            continue;
                        }
                        if (currentCategory != null)
                        {
                            if (categories.ContainsKey(currentCategory))
                            {
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

                        words.Add(new WordDto()
                        {
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                            Word = word,
                            WordRating = 1600,
                            LanguageType = type,
                            Attributes = attributes,
                            Translation = translation.ToLowerInvariant(),
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

        private void CreateAdmin(ApplicationContext ctx)
        {
            if (ctx.Users.Any()) return;

            var userResult = ctx.Users.Add(new UserDto()
            {
                IsAdmin = true,
                UserName = "Admin",
                Email = "admin@gmail.com"
            });

            var statResult = ctx.UserStatistics.Add(UserStatisticsDto.BuildForNewUser(userResult.Entity.Id));

            Console.WriteLine($"Creating admin...success");
        }

        private void LoadVerbos(List<WordDto> words)
        {
            var set = new HashSet<WordDto>();
            var filePath = "verbos.xlsx";

            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1); // First sheet

            foreach (var row in worksheet.RangeUsed().Rows())
            {
                if (row.RowNumber() <= 1) continue;

                var verb = new WordDto();
                var conjuaction = new WordConjugationModel();

                foreach (var cell in row.Cells())
                {
                    if (cell.Address.ColumnLetter == "A")
                    {
                        if (cell.Value.TryGetText(out var verbName))
                        {
                            verb.Word = verbName;
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb value name at: {cell.Address}");
                        }
                    }
                    if (cell.Address.ColumnLetter == "B")
                    {
                        if (cell.Value.TryGetText(out var traduccion))
                        {
                            verb.Translation = traduccion.ToLowerInvariant();
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb traduccion name at: {cell.Address}");
                        }
                    }

                    if (cell.Address.ColumnLetter == "C")
                    {
                        if (cell.Value.TryGetText(out var presente))
                        {
                            var items = presente.Split(",");
                            if (items.Length != 6) throw new Exception($"Unexpected size of presente at: {cell.Address}");
                            conjuaction.Presente = items.Select(v => v.Trim()).ToArray();
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb presente time at: {cell.Address}");
                        }
                    }

                    if (cell.Address.ColumnLetter == "D")
                    {
                        if (cell.Value.TryGetText(out var preteritoPerfecto))
                        {
                            var items = preteritoPerfecto.Split(",");
                            if (items.Length != 6) throw new Exception($"Unexpected size of pretetiro perfecto at: {cell.Address}");
                            conjuaction.PreteritoPerfecto = items.Select(v => v.Trim()).ToArray();
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb pretetiro perfecto at: {cell.Address}");
                        }
                    }

                    if (cell.Address.ColumnLetter == "E")
                    {
                        if (cell.Value.TryGetText(out var futuro))
                        {
                            var items = futuro.Split(",");
                            if (items.Length != 6) throw new Exception($"Unexpected size of futuro at: {cell.Address}");
                            conjuaction.FuturoSimple = items.Select(v => v.Trim()).ToArray();
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb futuro at: {cell.Address}");
                        }
                    }

                    if (cell.Address.ColumnLetter == "F")
                    {
                        if (cell.Value.TryGetText(out var indefinido))
                        {
                            var items = indefinido.Split(",");
                            if (items.Length != 6) throw new Exception($"Unexpected size of indefinido at: {cell.Address}");
                            conjuaction.PreteritoPerfectoIndefinido = items.Select(v => v.Trim()).ToArray();
                        }
                        else
                        {
                            throw new Exception($"Unexpected verb indefinido at: {cell.Address}");
                        }
                    }

                    if (cell.Address.ColumnLetter == "G")
                    {
                        if (cell.Value.TryGetText(out var gerundio))
                        {
                            conjuaction.Gerundio = gerundio.Trim();
                        }
                        else
                        {
                            conjuaction.Gerundio = null;
                        }
                    }

                }

                if (conjuaction.HasData()) verb.Conjugation = JsonConvert.SerializeObject(conjuaction);
                verb.CreatedTime = DateTime.UtcNow;
                verb.UpdatedTime = DateTime.UtcNow;
                verb.Attributes = "Verbos,Conjuaction";
                verb.LanguageType = WordLanguageType.SpanishRussian;
                verb.WordRating = 1600;

                if(words.Count(w => w.Word == verb.Word) > 0)
                {
                    Console.WriteLine($"DUBLICATE word: {verb.Word}");
                }
                else Console.WriteLine($"Added verb: {verb.Word}");
                words.Add(verb);

            }

        }
    }
}
