using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextAutoCompleteAPI.Models;

namespace TextAutoCompleteAPI.Controllers
{
    [Route("api/auto")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        [HttpPost]
        public IActionResult CreateAuto([FromBody] AutoComplete auto)
        {
            // Example: return full description
            string description = auto.getInput();
            Console.WriteLine(description);
            return Ok(new { Message = "Received", Description = description });
        }

    }
}
