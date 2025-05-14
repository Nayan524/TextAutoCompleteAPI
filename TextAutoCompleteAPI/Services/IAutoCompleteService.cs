using TextAutoCompleteAPI.Models;

namespace TextAutoCompleteAPI.Services
{
    public interface IAutoCompleteService
    {
        Task<List<string>> GetSuggestions(AutoComplete auto);
    }
}
