using Objects.Src.Primitives;
using System;

namespace Objects.Src.Models
{
    public class WordModel
    {
        public ulong Id { get; set; }

        public WordType Type { get; set; }

        public WordLevel Level { get; set; }

        public WordCategory Category { get; set; }

        public LanguageType LanguageFrom { get; set; }

        public LanguageType LanguageTo { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
