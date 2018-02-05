using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Models
{
    public class FirstOrSecondNameModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field can't be empty.")]
        [MinLength(length: 2, ErrorMessage = "Minimum length must be 2 characters.")]
        [MaxLength(length: 50, ErrorMessage = "Maximum length must be 50 characters.")]
        [RegularExpression(pattern: "^[a-zA-Z]+$", ErrorMessage = "Field value must contain only a-z and A-Z characters.")]
        public string FSName { get; set; }
    }
}