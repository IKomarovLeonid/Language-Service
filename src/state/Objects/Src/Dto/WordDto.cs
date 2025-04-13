using System;
using Objects.Src;

namespace Objects.Dto
{
    public class WordDto : IDto
    {
        public ulong Id { get; set; }

        public string Attributes { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public string Conjugation { get; set; }

        public double WordRating { get; set; }

        public WordLanguageType LanguageType { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public bool HasConjugation() => this.Conjugation != null;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var data = (WordDto)obj;
            return data.Word == this.Word;
        }
    }
}
