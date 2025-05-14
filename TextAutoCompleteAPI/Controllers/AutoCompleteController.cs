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
        //Declare the Interface 
        private readonly IAutoCompleteService _autoCompleteService;

        //Constructor Dependency Injection to instantaite AutoCompleteServiceAPI.cs
        public AutoCompleteController(IAutoCompleteService autoCompleteService)
        {
            _autoCompleteService = autoCompleteService;
        }

        //Define Method to accept POST request and will map the payload to Model class object. 
        [HttpPost]
        public async Task<IActionResult> CreateAuto([FromBody] AutoComplete auto)

        {
            //Adding validation for user input. 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Calling the Service Layer method to fetch suggestions. 
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