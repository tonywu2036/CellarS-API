namespace CellarS.Api.Models
{
    public class RephraseResponse
    {
        public string OriginalText { get; set; }
        public string RephrasedText { get; set;}
        public string Mode { get; set; }
        public string Provider { get; set; }
        public double? EstimatedLevel { get; set; } 
    }
}