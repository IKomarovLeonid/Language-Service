using Service.Models;
using System.Collections.Generic;

namespace Service.Src.Models
{
    public class WordsViewModel
    {
        public readonly Dictionary<string, string> Words;
        public readonly Language From;
        public readonly Language To;
        public readonly WordsType Type;

        public WordsViewModel(Dictionary<string, string> words, Language from, Language to, WordsType type)
        {
            this.Words = words;
            this.From = from;
            this.To = to;  
            this.Type = type;
        }
    }
}
