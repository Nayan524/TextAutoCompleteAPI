using TextAutoCompleteAPI.Models;

namespace TextAutoCompleteAPI.Services
{
    public interface AutoCompleteService
    {
        Task<List<string>> GetSuggestions(AutoComplete auto);
    }
}
