using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextAutoCompleteAPI.Models;
using OpenAI_API;
using OpenAI_API.Chat;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace TextAutoCompleteAPI.Controllers
{
    [Route("api/auto")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAuto([FromBody] AutoComplete auto)
        {
            // Example: return full description
            string description = auto.getInput();
            Console.WriteLine(description);

            try
            {
                // Get OpenAI API key from configuration
                var apiKey = _configuration["OpenAI:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    return StatusCode(500, new { Message = "OpenAI API key is not configured" });
                }

                // Initialize OpenAI client
                var openAi = new OpenAIAPI(apiKey);

                // Define the prompt for autocomplete suggestions
                var systemPrompt = "You are an autocomplete assistant. Given an incomplete input, provide 5 short (1-3 words) suggestions to naturally complete the input. Return the suggestions as a JSON array, e.g., [\"suggestion1\", \"suggestion2\", ...]. Do not include the original input in the suggestions.";
                var userPrompt = $"Input: {auto.Input}";

                // Create chat request for OpenAI
                var chatRequest = new ChatRequest
                {
                    Model = "gpt-3.5-turbo", // Use gpt-3.5-turbo for cost-efficiency
                    Messages = new[]
                    {
                        new ChatMessage(ChatMessageRole.System, systemPrompt),
                        new ChatMessage(ChatMessageRole.User, userPrompt)
                    },
                    MaxTokens = 50, // Limit response length
                    Temperature = 0.7, // Balance creativity and relevance
                    N = 1 // Single completion
                };

                // Call OpenAI API
                var response = await openAi.Chat.CreateChatCompletionAsync(chatRequest);
                var aiResponse = response.Choices[0].Message.Content.Trim();

                // Parse JSON array of suggestions
                List<string> suggestions;
                try
                {
                    suggestions = System.Text.Json.JsonSerializer.Deserialize<List<string>>(aiResponse) ?? new List<string>();
                }
                catch
                {
                    // Fallback: If JSON parsing fails, split by newlines or return empty list
                    suggestions = aiResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(s => s.Trim())
                                           .Where(s => !string.IsNullOrEmpty(s))
                                           .Take(5)
                                           .ToList();
                }

                // Log input and suggestions
                string input = auto.getInput();
                Console.WriteLine($"Input: {input}");
                Console.WriteLine($"Suggestions: {string.Join(", ", suggestions)}");

                // Return response
                return Ok(new
                {
                    Message = "Received",
                    Input = input,
                    Suggestions = suggestions
                });
            }
            catch (Exception ex)
            {
                // Log error (use a logging framework like Serilog in production)
                Console.WriteLine($"Error calling OpenAI API: {ex.Message}");
                return StatusCode(500, new { Message = "Error processing request", Error = ex.Message });
            }
            return Ok(new { Message = "Received", Description = description });
        }

    }
}
