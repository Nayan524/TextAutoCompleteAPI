using System;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using TextAutoCompleteAPI.Models;

namespace TextAutoCompleteAPI.Services
{
    public class AutoCompleteServiceAPI : AutoCompleteService
    {

        private readonly IOpenAIService _configuration;

        public ValuesController(IOpenAIService configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<string>> GetSuggestionsAsync(AutoComplete auto)
        {
            var completionRequest = new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage> {

                    ChatMessage.FromSystem("You are an autocomplete suggestion engine."),
                    ChatMessage.FromUser($"Provide 4 autocomplete suggestions for: \"{auto.GetInput}\"")


                },
                Model = "gpt-3.5-turbo",
                MaxTokens = 50,
                Temperature = 0.7f
            };
            var response = await _configuration.ChatCompletion.CreateCompletion(completionRequest);
            if (!response.Successful)
                return StatusCode((int)response.HttpStatusCode, response.Error?.Message);

            var suggestion = response.Choices?.FirstOrDefault()?.Message?.Content;
            return suggestion?
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

        }

    }
	
}



