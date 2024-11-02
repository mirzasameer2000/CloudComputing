
namespace PdfSummarizer_Backend.Controllers
{
    internal class PdfDocument : IDisposable
    {
        public PdfDocument(PdfReader pdfReader)
        {
            PdfReader = pdfReader;
        }

        public PdfReader PdfReader { get; }

        public void Dispose()
        {
            ((IDisposable)PdfReader).Dispose();
        }

        internal int GetNumberOfPages()
        {
            throw new NotImplementedException();
        }

        internal object GetPage(int page)
        {
            throw new NotImplementedException();
        }
    }
}