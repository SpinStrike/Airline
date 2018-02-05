using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Models
{
    public class EmailModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email field can't be empty.")]
        [RegularExpression(pattern: @"^[a-zA-Z0-9._-]{6,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid e-mail.")]
        public string Email { get; set; }
    }
}