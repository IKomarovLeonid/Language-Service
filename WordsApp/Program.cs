using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WordsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your name: ");
            var name = Console.ReadLine();

            Console.WriteLine("Select option for training: ");
            Console.WriteLine("1 -> Translate as palavras interrogativas (Russian -> Portugues)");
            Console.WriteLine("2 -> Translate os verbos (Russian -> Portugues)");
            Console.WriteLine("3 -> Translate os adjectivos (Russian -> Portugues)");

            int select = Convert.ToInt32(Console.ReadLine());

            switch(select)
            {
                case 1:
                    Preguntas(name);
                    break;
                case 2:
                    Verbos(name);
                    break;
                case 3:
                    Adjectivos(name);
                    break;
                default: Console.WriteLine($"Unknown option {select}");
                    break;
            }
        }

        static void Adjectivos(string userName)
        {
            var adjectivos = WordsProvider.GetAdjectivos();

            var info = ProcessLogic(adjectivos, 10);

            var fileName = userName.ToLowerInvariant() + "_adjectivos.txt";
            DataReader.SaveChanges(info, fileName);
            DataReader.ReadStats(fileName);
        }

        static void Verbos(string userName)
        {
            var verbos = WordsProvider.GetWerbos();

            var info = ProcessLogic(verbos, 10);

            var fileName = userName.ToLowerInvariant() + "_verbos.txt";
            DataReader.SaveChanges(info, fileName);
            DataReader.ReadStats(fileName);

        }

        private static void Preguntas(string userName)
        {
            var preguntas = WordsProvider.GetPreguntas();

            var info = ProcessLogic(preguntas, 10);

            var fileName = userName.ToLowerInvariant() + "_preguntas.txt";
            DataReader.SaveChanges(info, fileName);
            DataReader.ReadStats(fileName);
        }

        private static AttemptInfo ProcessLogic(Dictionary<string, string> wordsLinks, int correctAnswers) 
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            var rnd = new Random();
            int correct = 0;
            int count = 0;


            while (correct < correctAnswers)
            {
                count++;
                Console.WriteLine($"Correct answers: {correct} of total {correctAnswers}");
                var index = rnd.Next(wordsLinks.Count);
                var question = wordsLinks.ElementAt(index);
                Console.WriteLine($"Translate: {question.Value}");
                var answer = Console.ReadLine();
                if (string.Equals(answer.ToLowerInvariant(), question.Key.ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DisplayCorrect();
                    correct++;
                    continue;
                }
                DisplayIncorrect($"{question.Key}");
                Console.WriteLine(new string('-', 50));
            }

            Console.WriteLine($"Упражнение завершено: всего попыток: {count}; ошибок: {count - correctAnswers}; процент: {Math.Round((double) correct * 100 / count)}%");

            return new AttemptInfo()
            {
                Attempts = count,
                DateTime = DateTime.Now,
                Errors = count - correctAnswers,
                ExpectedCount = correctAnswers,
                Percent = Math.Round((double)(correct * 100 / count))
            };
        }

        private static void DisplayCorrect()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Correct!");
            Console.ResetColor();
        }

        private static void DisplayIncorrect(string asnwer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Incorrect! Answer is: {asnwer}");
            Console.ResetColor();
        }
    }
}
