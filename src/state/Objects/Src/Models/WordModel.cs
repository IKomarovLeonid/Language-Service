using Objects.Src.Primitives;
using System;

namespace Objects.Src.Models
{
    public class WordModel
    {
        public ulong Id { get; set; }

        public string Attributes { get; set; }

        public string Word { get; set; }

        public string Conjugation { get; set; }

        public string[] Translations { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
