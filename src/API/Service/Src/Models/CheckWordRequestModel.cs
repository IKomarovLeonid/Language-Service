using Service.Models;

namespace Service.Src.Models
{
    public class CheckWordRequestModel
    {
        public Language from { get; set; }
        public string FromValue { get; set; }
        public Language To { get; set; }
        public string ToValue { get; set; }
        public WordsType Type { get; set; }
    }
}
