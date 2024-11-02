

namespace PdfSummarizer_Backend.Controllers
{
    internal class PdfReader : IDisposable
    {
        public PdfReader(MemoryStream memoryStream)
        {
            MemoryStream = memoryStream;
        }

        public MemoryStream MemoryStream { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}