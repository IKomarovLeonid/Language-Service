namespace Objects.Src.Models
{
    public class AttemptModel
    {
        public string Word { get; set; }

        public string UserTranslation { get; set; }

        public string ExpectedTranslation { get; set; }

        public double TotalSeconds { get; set; }

        public bool IsCorrect { get; set; }
    }
}
