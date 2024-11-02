namespace PdfSummarizer_Backend.Controllers
{
    internal class CompletionRequest
    {
        public string Prompt { get; set; }
        public object Model { get; set; }
        public int MaxTokens { get; set; }
    }
}