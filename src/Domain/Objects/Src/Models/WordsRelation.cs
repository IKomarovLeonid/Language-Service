namespace Service.Models
{
    public class WordsRelation
    {
        public Language From { get; set; }

        public string ValueFrom { get; set; }

        public Language To { get; set; }

        public string ValueTo { get; set; }

        public WordsType Type { get; set; }
    }
}
