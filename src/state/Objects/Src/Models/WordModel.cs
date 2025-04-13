using Objects.Src.Models;
using System;

namespace Objects.Models
{
    public class WordModel
    {
        public ulong Id { get; set; }

        public string Attributes { get; set; }

        public string Word { get; set; }

        public WordConjugationModel Conjugation { get; set; }

        public double WordRating { get; set; }

        public double TotalAttempts { get; set; }

        public double SuccessRate { get; set; }

        public string[] Translations { get; set; }

        public WordLanguageType LanguageType { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
