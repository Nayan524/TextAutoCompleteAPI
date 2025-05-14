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
        private readonly ILoggerService _loggerService;

        //Constructor Dependency Injection to instantaite AutoCompleteServiceAPI.cs
        public AutoCompleteController(IAutoCompleteService autoCompleteService, ILoggerService loggerService)
        {
            _autoCompleteService = autoCompleteService;
            _loggerService = loggerService;
        }

        //Define Method to accept POST request and will map the payload to Model class object. 
        [HttpPost]
        public async Task<IActionResult> CreateAuto([FromBody] AutoComplete auto)

        {
            await _loggerService.LogApi(auto.GetInput(), 200, "Received Input");
            //Adding validation for user input. 
            if (!ModelState.IsValid)
            {
                await _loggerService.LogApi(auto.GetInput(), 400, "Invalid model state");
                return BadRequest(ModelState);
            }
            try
            {
                //Calling the Service Layer method to fetch suggestions. 
                var suggestions = await _autoCompleteService.GetSuggestions(auto);
                await _loggerService.LogApi(auto.GetInput(), 200, "Success");
                return Ok(new { suggestions });

            }
            catch (Exception exception)
            {
                await _loggerService.LogApi(auto.GetInput(), 500, exception.Message);
                return StatusCode(500, exception.Message);
            }
           
        }
    }
}