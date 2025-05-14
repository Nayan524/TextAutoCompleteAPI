using Microsoft.AspNetCore.Mvc;
using TextAutoCompleteAPI.Models;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System.Threading.Tasks;
using OpenAI.Interfaces;
using TextAutoCompleteAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace TextAutoCompleteAPI.Controllers
{
    [Route("api/auto")]
    [ApiController]
    public class AutoCompleteController : ControllerBase
    {
        private readonly IAutoCompleteService _autoCompleteService;

        public AutoCompleteController(IAutoCompleteService autoCompleteService)
        {
            _autoCompleteService = autoCompleteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuto([FromBody] AutoComplete auto)

        {
            Console.WriteLine(auto.GetInput());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var suggestions = await _autoCompleteService.GetSuggestions(auto);
                return Ok(new { suggestions });

            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
           
        }
    }
}