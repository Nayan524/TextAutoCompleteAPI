using System;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using TextAutoCompleteAPI.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace TextAutoCompleteAPI.Services
{
    public class AutoCompleteServiceAPI : IAutoCompleteService
    {

        //Declaring HttpClient to call NLP Service
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        //Below is known as Constructor Dependency Injection which creates instance of HttpClient and InMemory cache at runtime. 
        public AutoCompleteServiceAPI(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        //Defining the method to process the user prompt and returing the suggestions
        public async Task<List<string>> GetSuggestions(AutoComplete auto)
        {
            var prompt = Uri.EscapeDataString(auto.GetInput());
            
            //To check if we have cached suggestions for the prompt. 
            if (_memoryCache.TryGetValue(prompt, out List<string> cachedSuggestions))
                return cachedSuggestions;
            
            var url = $"https://api.datamuse.com/sug?s={prompt}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get suggestions");

            //Fetching the response from API
            var json = await response.Content.ReadAsStringAsync();   
              

            var results = JsonSerializer.Deserialize<List<Response>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            //To verify if API returned the results or not
            if (results != null)
            {
                foreach (var item in results)
                {
                    Console.WriteLine($"Word: {item.Word}, Score: {item.Score}");
                }
            }
            else
            {
                Console.WriteLine("Results is null");
            }

            var suggestionText = results?
                .Select(r => r.Word)
                .Take(5)
                .ToList() ?? new List<string>();

            //Cache the results with a 5 minute expiration
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(prompt, suggestionText, cacheEntryOptions);
            return suggestionText;

        }

    }
	
}



