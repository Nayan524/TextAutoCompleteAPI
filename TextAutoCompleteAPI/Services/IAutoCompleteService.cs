using TextAutoCompleteAPI.Models;

namespace TextAutoCompleteAPI.Services
{
    //Creating interface for Service Class. Multiple Service class can exist which implements the interface. 
    public interface IAutoCompleteService
    {
        Task<List<string>> GetSuggestions(AutoComplete auto);
    }
}
