using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Files;
using OpenAI.Chat;
using OpenAI.Models;
using OpenAI;
using iTextSharp.text.pdf.parser;
using PdfSummarizer_Backend.Controllers;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly string apiKey = "YOUR_OPENAI_API_KEY"; // Replace with your OpenAI API key

        [HttpPost("summarize-pdf")]
        public async Task<IActionResult> SummarizePdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Step 1: Extract text from the uploaded PDF file
                string extractedText = await ExtractTextFromPdf(file);

                // Step 2: Summarize the extracted text
                string summary = await SummarizeTextAsync(extractedText);

                // Return the summary as a JSON response
                return Ok(new { Summary = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing file: {ex.Message}");
            }
        }

        private async Task<string> ExtractTextFromPdf(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copy the uploaded PDF file to a MemoryStream
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Rewind the memory stream to the beginning

                // Create a PDF reader and extract text
                using (var pdfReader = new PdfReader(memoryStream))
                {
                    using (var pdfDocument = new PdfDocument(pdfReader))
                    {
                        var text = new StringWriter();
                        for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                        {
                            var pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page));
                            text.WriteLine(pageText);
                        }

                        return text.ToString(); // Return the extracted text
                    }
                }
            }
        }

        private async Task<string> SummarizeTextAsync(string extractedText)
        {
            var openAiApi = new OpenAIAPI(apiKey);

            // Create a completion request
            var request = new CompletionRequest
            {
                Prompt = $"Summarize the following text:\n{extractedText}",
                Model = "text-davinci-003", // Ensure the model is valid
                MaxTokens = 100
            };

            try
            {
                // Make the API call
                var completion = await openAiApi.Completions.CreateAsync(request);

                // Ensure that the completion result is not null and has at least one choice
                if (completion.Choices != null && completion.Choices.Count > 0)
                {
                    return completion.Choices[0].Text.Trim(); // Return the summary text
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return $"An error occurred while summarizing: {ex.Message}";
            }

            // Handle case where no completion is returned
            return "No summary could be generated.";
        }
    }
}
