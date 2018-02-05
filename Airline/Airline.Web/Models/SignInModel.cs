using System;
using System.ComponentModel.DataAnnotations;


namespace Airline.Web.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email field can't be empty.")]
        [RegularExpression(pattern: @"^[a-zA-Z0-9._-]{6,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field can't be empty.")]
        [RegularExpression(pattern: @"^(?=.*[0-9]+)(?=.*[a-z]+)(?=.*[A-Z]+)[a-zA-Z0-9]{6,12}$", ErrorMessage = "Invalid password.")]
        public string Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}