using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Areas.Admin.Models
{
    public class ProfessionCreationModel
    {
        [Required(ErrorMessage = "Profession name field can't be empty.")]
        [MinLength(length: 2, ErrorMessage = "Profession name must contain minimum 3 characters.")]
        public string Name { get; set; }
    }
}