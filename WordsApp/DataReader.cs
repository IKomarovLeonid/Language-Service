using System;
using System.Globalization;
using System.IO;

namespace WordsApp
{
    internal class DataReader
    {
        public static void SaveChanges(AttemptInfo info, string fileName)
        {
            File.AppendAllText(fileName, info.ToString() + Environment.NewLine);
        }

        public static void ReadStats(string file)
        {
            if (File.Exists(file))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Print your statistics: ");
                Console.ResetColor();
                using (StreamReader reader = File.OpenText(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line into values
                        string[] values = line.Split(',');

                        if (values.Length == 5)
                        {
                            // Parse values and create an AttemptInfo instance
                            AttemptInfo attempt = new AttemptInfo
                            {
                                ExpectedCount = int.Parse(values[0]),
                                Attempts = int.Parse(values[1]),
                                Errors = int.Parse(values[2]),
                                Percent = double.Parse(values[3], CultureInfo.InvariantCulture),
                                DateTime = DateTime.ParseExact(values[4], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                            };

                            Console.WriteLine(attempt.Print());
                        }
                        else
                        {
                            Console.WriteLine("Invalid data format in the file.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
    }
}
