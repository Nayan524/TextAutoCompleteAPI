using System.ComponentModel.DataAnnotations;
namespace TextAutoCompleteAPI.Models
{
    public class AutoComplete

    {

        //Adding validations for user prompt
        [Required(ErrorMessage = "Input cannot be empty.")]
        [MinLength(3, ErrorMessage = "Input must be at least 3 characters.")]
        [MaxLength(100, ErrorMessage = "Input must not exceed 200 characters.")]
        public string Input { get; set; }

        public string GetInput()
        {
            return $"{Input}";
        }

    }
}
