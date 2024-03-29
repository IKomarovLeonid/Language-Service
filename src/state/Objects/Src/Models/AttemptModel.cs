﻿namespace Objects.Src.Models
{
    public class AttemptModel
    {
        public string Word { get; set; }

        public string UserTranslation { get; set; }

        public string[] ExpectedTranslations { get; set; }

        public double TotalSeconds { get; set; }

        public bool IsCorrect { get; set; }
    }
}
