namespace Objects.Src.Models
{
    public class WordConjugationModel
    {
        public string[] Presente { get; set; }

        public string[] PreteritoPerfecto { get; set; }

        public string[] FuturoSimple { get; set; }

        public string[] PreteritoPerfectoIndefinido { get; set; }

        public bool HasData() => this.PreteritoPerfectoIndefinido != null || this.FuturoSimple != null || this.PreteritoPerfecto != null || this.Presente != null;

    }
}
