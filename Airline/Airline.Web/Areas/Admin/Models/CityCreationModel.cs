using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Areas.Admin.Models
{
    public class CityCreationModel
    {
        [Required(ErrorMessage = "City name field can't be empty.")]
        [MinLength(length: 2, ErrorMessage = "Minimum length of city name is 2 characters.")]
        [MaxLength(length: 50, ErrorMessage = "Maximum length of city name id 50 characters.")]
        [RegularExpression(pattern: "^[a-zA-Z]+$", ErrorMessage = "City name must contain only a-z and A-Z characters.")]
        public string Name { get; set; }
    }
}